using System.Collections;
using System.Collections.Generic;
using UFO_PlayerStuff;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DoctorsDoor : MonoBehaviour, I_Interactable
{
    public UnityEvent StartedDoorInteractedWith;

    public UnityEvent ScreenBlackDuringHandleInteractionEnter;
    public void HandleInteraction(CameraForwardsSampler playerCamSampler)
    {
        if (playerCamSampler.InteractableObjectInRangeRef != this.gameObject)
            return;
        
        this.StartedDoorInteractedWith?.Invoke();
        
        Invoke(nameof(this.HandleInteractionWhileScreenBlack), 0.2f);
        
        Debug.Log("Door Interacted with");
    }

    public void HandleInteractionWhileScreenBlack()
    {
        this.ScreenBlackDuringHandleInteractionEnter?.Invoke();
    }
}
