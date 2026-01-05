using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReturnFromSettingsButton : MonoBehaviour
{
    [SerializeField] private GameObject nameInput;
        
        [SerializeField] private GameObject PlayButton;
    
        [SerializeField] private GameObject settingsButton;
    
        [SerializeField] private GameObject CreditsButton;
    
        [SerializeField] private GameObject settingsBackgroundPanel;
    
        [SerializeField] private AnimationCurve playAndCreditsLeaveAnimCurve;
    
        private EventSystem eventSystem;


        private Vector3 nameInputOGPos, playButtonOgPos, settingsButtonOgPos, CreditsButtonOgPos;
        
        private void Awake()
            {
                this.eventSystem = EventSystem.current;

                this.nameInputOGPos = this.nameInput.transform.position;
                this.playButtonOgPos = this.PlayButton.transform.position;
                this.settingsButtonOgPos = this.settingsButton.transform.position;
                this.CreditsButtonOgPos = this.CreditsButton.transform.position;
            }
        public void ClickedButton()
        {
            //move the input button
            Vector3 oldPos = this.nameInput.transform.position;
            this.nameInput.transform.position.LerpTo(new Vector3(nameInputOGPos.x, oldPos.y, oldPos.z), 0.6f, value => this.nameInput.transform.position = value, null, this.playAndCreditsLeaveAnimCurve);
            //move the playbutton left off the screen
            oldPos = this.PlayButton.transform.position;
            this.PlayButton.transform.position.LerpTo(new Vector3(playButtonOgPos.x, oldPos.y, oldPos.z), 0.6f,
                value => { this.PlayButton.transform.position = value; },
                pkg => { },
                this.playAndCreditsLeaveAnimCurve);
    
            oldPos = this.settingsButton.transform.position;
            this.settingsButton.transform.position.LerpTo(new Vector3(settingsButtonOgPos.x, oldPos.y, oldPos.z), 0.6f,
                value => { this.settingsButton.transform.position = value; }, null, this.playAndCreditsLeaveAnimCurve);
    
            //Move the credits button left
            oldPos = this.CreditsButton.transform.position;
            this.CreditsButton.transform.position.LerpTo(new Vector3(CreditsButtonOgPos.x, oldPos.y, oldPos.z), 0.6f,
                value => { this.CreditsButton.transform.position = value; } , pkg =>
                {
                }, this.playAndCreditsLeaveAnimCurve);

            this.settingsBackgroundPanel.transform.position.LerpTo(
                new Vector3(Screen.width + (Screen.width / 2), Screen.height / 2, 0), 1f,
                val => this.settingsBackgroundPanel.transform.position = val);
    
            this.eventSystem.SetSelectedGameObject(this.settingsButton);
        }
}
