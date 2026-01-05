using System;
using System.Collections;
using System.Collections.Generic;
using UFO_PlayerStuff;
using UnityEngine;
using UnityEngine.Events;


public class OneShotInteractable : MonoBehaviour, I_Interactable
{
    public UnityEvent interacted;

    public DelayedUnityEvent[] delayedEvents;

    private bool hasBeenInteractedWith = false;

    public void HandleInteraction(CameraForwardsSampler playerCamSampler)
    {
        if (this.hasBeenInteractedWith)
            return;
        
        if (playerCamSampler.InteractableObjectInRangeRef != this.gameObject)
            return;
        
        this.interacted?.Invoke();
        
        foreach (DelayedUnityEvent evt in this.delayedEvents)
        {
            evt.Go();
        }
    }
}
