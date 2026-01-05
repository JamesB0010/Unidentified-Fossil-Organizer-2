using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NameInputField : MonoBehaviour, IMoveHandler
{
    [SerializeField] private GameObject inputField;

    [SerializeField] private GameObject PlayButton;

    [SerializeField] private GameObject settingsButton;

    [SerializeField] private GameObject CreditsButton;

    private EventSystem eventSystem;

    private AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField] private GameObject keyboardParent;

    [SerializeField] private GameObject QButton;

    [SerializeField] private AnimationCurve playAndCreditsLeaveAnimCurve;
    
    public UnityEvent ActivateKeyboardEvent;


    private void Awake()
    {
        this.eventSystem = EventSystem.current;
    }

    public void ActivateKeyboard()
    {
        this.ActivateKeyboardEvent?.Invoke();
        //Move Input field into the top and center
        inputField.transform.position.LerpTo(new Vector3(Screen.width / 2, -200 + Screen.height, 0), 0.6f,
            value => { inputField.transform.position = value; }, pkg => { },
            this.animCurve);


        //move the playbutton left off the screen
        Vector3 oldPos = this.PlayButton.transform.position;
        this.PlayButton.transform.position.LerpTo(new Vector3(-442.99f, oldPos.y, oldPos.z), 0.6f,
            value => { this.PlayButton.transform.position = value; },
            pkg => { },
            this.playAndCreditsLeaveAnimCurve);

        oldPos = this.settingsButton.transform.position;
        this.settingsButton.transform.position.LerpTo(new Vector3(-442.99f, oldPos.y, oldPos.z), 0.6f,
            value => { this.settingsButton.transform.position = value; }, null, this.playAndCreditsLeaveAnimCurve);

        //Move the credits button left
        oldPos = this.CreditsButton.transform.position;
        this.CreditsButton.transform.position.LerpTo(new Vector3(-442.99f, oldPos.y, oldPos.z), 0.6f,
            value => { this.CreditsButton.transform.position = value; },
            pkg =>
            {
                //move the keyboard center
                RectTransform keyboardParentRectTransform = (RectTransform)this.keyboardParent.transform;
                keyboardParentRectTransform.anchoredPosition.LerpTo(new Vector3(0, 374, 0), 0.8f, val => keyboardParentRectTransform.anchoredPosition = val, pkg => this.eventSystem.SetSelectedGameObject(QButton), this.animCurve);
            },
            this.playAndCreditsLeaveAnimCurve);
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            return;

        float verticalInput = Input.GetAxis("VerticalMenu");
        if (EventSystem.current.currentSelectedGameObject == inputField)
        {
            //move off input field
            if (verticalInput < -0.2f)
            {
                this.eventSystem.SetSelectedGameObject(this.PlayButton);
            }

            if (Input.GetAxis("Submit") == 1.0f)
            {
                this.ActivateKeyboard();
                Debug.Log("start typing");
                this.inputField.GetComponent<TMP_InputField>().text = "";
            }
        }
    }
    public void OnMove(AxisEventData eventData)
    {
        Debug.Log("On move");
        if (EventSystem.current.currentSelectedGameObject == null)
                    return;

        if (eventData.moveDir != MoveDirection.Down)
            return;
        
        Debug.Log("move down");
    }
}
