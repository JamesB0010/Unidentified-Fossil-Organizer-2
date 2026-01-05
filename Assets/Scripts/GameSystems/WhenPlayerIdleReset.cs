using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class WhenPlayerIdleReset : MonoBehaviour
{
     private float LastImputTimeStamp;

     private float currentTime;

     private float inactivityThreshold = 20.0f;

     private bool inactivityNotified = false;

     private bool currentlyActive = false;

     [SerializeField] private UnityEvent InactivityDetected, ActivityDetected;
     private void Start()
     {
          this.LastImputTimeStamp = Time.time;
     }

     private void Update()
     {
          this.currentTime = Time.time - this.LastImputTimeStamp;

          if (this.currentTime >= this.inactivityThreshold && inactivityNotified == false)
          {
               if (this.currentlyActive)
               {
                    this.LastImputTimeStamp = Time.time;
                    return;
               }

               this.inactivityNotified = true;
               InactivityDetected.Invoke();
          }
     }

     public void PlayerHasDoneSomething()
     {
          this.currentlyActive = true;
          this.inactivityNotified = false;
          this.LastImputTimeStamp = Time.time;
          this.ActivityDetected.Invoke();
     }

     public void PlayerStoppedMoving()
     {
          this.currentlyActive = false;
     }
}
