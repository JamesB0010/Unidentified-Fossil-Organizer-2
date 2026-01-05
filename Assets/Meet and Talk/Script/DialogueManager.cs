using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using MeetAndTalk.GlobalValue;
using MeetAndTalk.Localization;

namespace MeetAndTalk
{
    public class DialogueManager : DialogueGetData
    {
        [HideInInspector] public static DialogueManager Instance;
        public LocalizationManager localizationManager;

        public UnityEvent StartDialogueEvent;
        public UnityEvent EndDialogueEvent;

        public BaseNodeData currentDialogueNodeData;
        private BaseNodeData lastDialogueNodeData;

        private TimerChoiceNodeData _nodeTimerInvoke;
        private DialogueNodeData _nodeDialogueInvoke;
        private DialogueChoiceNodeData _nodeChoiceInvoke;

        private List<Coroutine> activeCoroutines = new List<Coroutine>();

        float Timer;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            Timer -= Time.deltaTime;
        }

        public void ChangeUI(DialogueUIManager UI)
        {
            // Setup UI
            if (UI != null) DialogueUIManager.Instance = UI;
            else Debug.LogError("DialogueUIManager.UI Object jest Pusty!");
        }

        public void SetupDialogue(DialogueContainerSO dialogue)
        {
            if (dialogue != null) dialogueContainer = dialogue;
            else Debug.LogError("DialogueContainerSO.dialogue Object jest Empty!");
        }
        public void StartDialogue(DialogueContainerSO dialogue) { StartDialogue(dialogue, ""); }
        public void StartDialogue(string ID) { StartDialogue(null, ID); }
        public void StartDialogue() { StartDialogue(null, ""); }

        public void StartDialogue(DialogueContainerSO DialogueSO, string StartID)
        {
            // Setup Dialogue (if not empty)
            if (DialogueSO != null) { SetupDialogue(DialogueSO); }
            // Error: No Setup Dialogue
            if (dialogueContainer == null) { Debug.LogError("Error: Dialogue Container SO is not assigned!"); }

            // Check ID
            if (dialogueContainer.StartNodeDatas.Count == 0) { Debug.LogError("Error: No Start Node in Dialogue Container!"); }

            BaseNodeData _start = null;

                _start = dialogueContainer.StartNodeDatas[Random.Range(0, dialogueContainer.StartNodeDatas.Count)];
            

            CheckNodeType(GetNextNode(_start));

            StartDialogueEvent.Invoke();
        }


        public void CheckNodeType(BaseNodeData _baseNodeData)
        {
            switch (_baseNodeData)
            {
                case StartNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case DialogueNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case DialogueChoiceNodeData nodeData:
                    RunNode(nodeData);
                    break;
                case EndNodeData nodeData:
                    RunNode(nodeData);
                    break;
                default:
                    break;
            }
        }


        private void RunNode(StartNodeData _nodeData)
        {
            CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
        }

        public UnityEvent NodeRan;
        private void RunNode(DialogueNodeData _nodeData)
        {
            lastDialogueNodeData = currentDialogueNodeData;
            currentDialogueNodeData = _nodeData;

            MakeButtons(new List<DialogueNodePort>());

            _nodeDialogueInvoke = _nodeData;

            StopAllTrackedCoroutines();
            
            this.NodeRan?.Invoke();
            //IEnumerator tmp() { yield return new WaitForSeconds(_nodeData.Duration); DialogueNode_NextNode(); }
            //if (_nodeData.Duration != 0) StartTrackedCoroutine(tmp()); ;
        }
        private void RunNode(DialogueChoiceNodeData _nodeData)
        {
            lastDialogueNodeData = currentDialogueNodeData;
            currentDialogueNodeData = _nodeData;

            // Normal Multiline
            MakeButtons(new List<DialogueNodePort>());

            _nodeChoiceInvoke = _nodeData;

            StopAllTrackedCoroutines();
            IEnumerator tmp() { yield return new WaitForSeconds(_nodeData.Duration); ChoiceNode_GenerateChoice(); }
            StartTrackedCoroutine(tmp()); 
            
            
            this.NodeRan?.Invoke();
        }

        private void RunNode(EndNodeData _nodeData)
        {
            switch (_nodeData.EndNodeType)
            {
                case EndNodeType.End:
                    EndDialogueEvent.Invoke();
                    break;
                case EndNodeType.Repeat:
                    CheckNodeType(GetNodeByGuid(currentDialogueNodeData.NodeGuid));
                    break;
                case EndNodeType.GoBack:
                    CheckNodeType(GetNodeByGuid(lastDialogueNodeData.NodeGuid));
                    break;
                case EndNodeType.ReturnToStart:
                    CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[Random.Range(0,dialogueContainer.StartNodeDatas.Count)]));
                    break;
                default:
                    break;
            }
        }


        private void MakeButtons(List<DialogueNodePort> _nodePorts)
        {
            List<string> texts = new List<string>();
            List<UnityAction> unityActions = new List<UnityAction>();

            foreach (DialogueNodePort nodePort in _nodePorts)
            {
                texts.Add(nodePort.TextLanguage.Find(text => text.languageEnum == localizationManager.SelectedLang()).LanguageGenericType);
                UnityAction tempAction = null;
                tempAction += () =>
                {
                    CheckNodeType(GetNodeByGuid(nodePort.InputGuid));
                };
                unityActions.Add(tempAction);
            }

        }


        void DialogueNode_NextNode() { CheckNodeType(GetNextNode(_nodeDialogueInvoke)); }

        public BaseNodeData GetNextDialougeNode()
        {
            return GetNextNode(_nodeDialogueInvoke);

        }
        void ChoiceNode_GenerateChoice() { MakeButtons(_nodeChoiceInvoke.DialogueNodePorts);
        }

        #region Improve Coroutine
        private void StopAllTrackedCoroutines()
        {
            foreach (var coroutine in activeCoroutines)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
            }
            activeCoroutines.Clear();
        }

        private Coroutine StartTrackedCoroutine(IEnumerator coroutine)
        {
            Coroutine newCoroutine = StartCoroutine(coroutine);
            activeCoroutines.Add(newCoroutine);
            return newCoroutine;
        }
        #endregion

        public void SkipDialogue(int indexToSkipTo = 0)
        {
            StopAllTrackedCoroutines();

            switch (currentDialogueNodeData)
            {
                case DialogueNodeData nodeData:
                    DialogueNode_NextNode();
                    break;
                case DialogueChoiceNodeData nodeData:
                    CheckNodeType(GetNodeByGuid(nodeData.DialogueNodePorts[indexToSkipTo].InputGuid));
                    break;
                default:
                    break;
            }
        }
        public void ForceEndDialog()
        {
            EndDialogueEvent.Invoke();


            StopAllTrackedCoroutines();
        }
    }
}
