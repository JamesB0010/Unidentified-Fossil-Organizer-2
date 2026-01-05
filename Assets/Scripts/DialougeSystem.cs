using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using MeetAndTalk;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class DialougeSystem : MonoBehaviour
{
    
    [SerializeField] private DialogueContainerSO dialouge;
    private VisualElement uiRoot;

    private VisualElement arrowTopBuffer;

    private TextElement dialougeTalkingText;

    private TextElement[] playerResponseTexts = new TextElement[3];

    private VisualElement[] playerResponseTracks = new VisualElement[3];

    private int currentlyFocusedDialougeOption = 0;
    private DialogueManager dialougeManager;

    [SerializeField] private UnityEvent startedExitDialouge;

    [SerializeField] private UnityEvent screenBlackExitDialouge;

    [SerializeField] private DoctorsDoor doctorsDoor;
    
    private int maxOptionIndex = 2;

    private bool locked = true;

    [SerializeField] private GameObject closeUpOnDoorCamera;

    [SerializeField] private UnityEvent playAlienSpeach;

    [SerializeField] private StudioEventEmitter fmodEventEmitter;

    private int CurrentlyFocusedDialougeOption
    {
        get => this.currentlyFocusedDialougeOption;
        set
        {
            if (this.locked)
                return;
            
            bool noChangeObserved = this.currentlyFocusedDialougeOption == value;
            if (noChangeObserved)
                return;

            int oldVal = this.currentlyFocusedDialougeOption;
            this.currentlyFocusedDialougeOption = Mathf.Min(this.maxOptionIndex,Mathf.Max(0,value)); // the mathf stuff clamps the value to be between 0 and 2

            this.arrowTopBuffer.style.height = Length.Percent(73.5f + (10f * this.currentlyFocusedDialougeOption));
            this.playerResponseTexts[oldVal].RemoveFromClassList("HoveredResponseText");
            this.playerResponseTexts[this.currentlyFocusedDialougeOption].AddToClassList("HoveredResponseText");
        }
    }
    private void Awake()
    {
        SetupDependencies();
    }

    private void Start()
    {
        this.dialougeManager = GetComponent<DialogueManager>();
        this.dialougeManager.SetupDialogue(this.dialouge);
        this.StartDialouge();
    }

    public void StartDialouge()
    {
        try
        {
            this.dialougeManager.StartDialogue();
            this.SetupDependencies();
            this.CurrentlyFocusedDialougeOption = 0;
        }
        catch {
        }
    }

    private void HideResponses()
    {
        for (int i = 0; i < this.playerResponseTracks.Length; i++)
        {
            this.playerResponseTracks[i].style.opacity = new StyleFloat()
            {
                value = 0
            };
        }
    }

    private void ReactToDialouge()
    {
        Debug.Log("dialouge");
        this.fmodEventEmitter.Stop();
        switch (this.dialougeManager.currentDialogueNodeData)
        {
            case DialogueChoiceNodeData data:
                this.playerResponseTracks[1].style.display = DisplayStyle.Flex;
                this.playerResponseTracks[2].style.display = DisplayStyle.Flex;
                this.maxOptionIndex = 2;
                this.SetMainBodyText(data.TextType[0].LanguageGenericType);
                for (int i = 0; i < data.DialogueNodePorts.Count; i++)
                { 
                    this.SetDialougeOptionText(data.DialogueNodePorts[i].TextLanguage[0].LanguageGenericType, this.playerResponseTexts[i]);
                }
                Invoke(nameof(this.PlayChildDirector), 0.3f);
                this.fmodEventEmitter.EventReference = data.FmodEventRef.eventReference;
                this.fmodEventEmitter.Lookup();
               this.fmodEventEmitter.Play(); 
                break;
            case DialogueNodeData data:
                this.playerResponseTracks[1].style.display = DisplayStyle.None;
                this.playerResponseTracks[2].style.display = DisplayStyle.None;
                this.playerResponseTexts[0].text = "next";
                this.CurrentlyFocusedDialougeOption = 0;
                this.maxOptionIndex = 0;
                this.SetMainBodyText(data.TextType[0].LanguageGenericType);
                this.fmodEventEmitter.EventReference = data.FmodEventRef.eventReference;
                this.fmodEventEmitter.Lookup();
                this.fmodEventEmitter.Play();
                break;
            default:
                break;
        }
        
        this.PlayChildDirector();
    }

    private void PlayChildDirector()
    {
        var director = GetComponentInChildren<PlayableDirector>();
        director.Stop();
        director.Play();
        

        foreach (var responseBackground in this.playerResponseTracks)
        {
            responseBackground.style.opacity.value.LerpTo(1f, 0.8f, value =>
            {
                responseBackground.style.opacity = new StyleFloat()
                {
                    value = value
                };
            });
        }
    }

    public void SetMainBodyText(string text)
    {
        StartCoroutine(TypeWriteText(text, this.dialougeTalkingText));
    }

    private IEnumerator TypeWriteText(string text, TextElement toWriteTo)
    {
        float timeElapsed = 0;
        float minimumTime = 1.0f;
        
        this.locked = true;
        this.closeUpOnDoorCamera.SetActive(true);
        int index = 0;
        while (index <= text.Length)
        {
            toWriteTo.text = text.Substring(0, index);
            index++;
            yield return new WaitForSeconds(0.1f);
            timeElapsed += 0.1f;
        }

        if (timeElapsed < minimumTime)
        {
            float delta = minimumTime - timeElapsed;
            yield return new WaitForSeconds(delta);
        }
        this.locked = false;

        bool backUpCamera = true;

        switch (this.dialougeManager.currentDialogueNodeData)
        {
            case DialogueNodeData data:
                if (this.dialougeManager.GetNextDialougeNode() is EndNodeData)
                    backUpCamera = false;
                break;
        }

        if(backUpCamera)
            this.closeUpOnDoorCamera.SetActive(false);
    }

    public void SetDialougeOptionText(string text, TextElement dialougeText)
    {
        dialougeText.text = text;
    }

    private void SetupDependencies()
    {
        this.uiRoot = GetComponent<UIDocument>().rootVisualElement;
        arrowTopBuffer = this.uiRoot.Q<VisualElement>("UserPrompts").Q<VisualElement>("TopBuffer");
        this.dialougeTalkingText = this.uiRoot.Q<TextElement>("DialougeTalkingText");

        this.playerResponseTexts[0] = this.uiRoot.Q<TextElement>("Choice1Text");
        this.playerResponseTexts[1] = this.uiRoot.Q<TextElement>("Choice2Text");
        this.playerResponseTexts[2] = this.uiRoot.Q<TextElement>("Choice3Text");

        this.playerResponseTracks[0] = this.uiRoot.Q<VisualElement>("Choice1Track");
        this.playerResponseTracks[1] = this.uiRoot.Q<VisualElement>("Choice2Track");
        this.playerResponseTracks[2] = this.uiRoot.Q<VisualElement>("Choice3Track");
    }

    public void NavigateUi(InputAction.CallbackContext ctx)
    {
        float direction = ctx.ReadValue<Vector2>().y;
        this.CurrentlyFocusedDialougeOption -= (int)direction;
    }

    public void SelectOption(InputAction.CallbackContext ctx)
    {
        if (this.locked)
            return;
        
        if (!ctx.performed)
            return;
        
        this.HideResponses();
        StartCoroutine(nameof(AlienSpeakThenContinue));
    }

    private IEnumerator AlienSpeakThenContinue()
    {
        if (this.dialougeManager.currentDialogueNodeData is DialogueChoiceNodeData)
        {
            this.dialougeTalkingText.text = this.playerResponseTexts[this.CurrentlyFocusedDialougeOption].text;
            this.playAlienSpeach?.Invoke();
            yield return new WaitForSeconds(2);
        }
        this.dialougeManager.SkipDialogue(this.currentlyFocusedDialougeOption);
        yield return null;
    }

    public void OnDialougeRan()
    {
        this.ReactToDialouge();
    }

    public void EndDialouge()
    {
        this.startedExitDialouge?.Invoke();
        Invoke(nameof(this.ScreenBlackDoExitDialouge), 0.2f);
        this.dialougeManager.SetupDialogue(this.dialouge);
    }

    private void ScreenBlackDoExitDialouge()
    {
        this.screenBlackExitDialouge?.Invoke();
    }
}
