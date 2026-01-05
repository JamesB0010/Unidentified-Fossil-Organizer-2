using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

class AbleToInteractStateData
{
    public AbleToInteractStateData(bool interactButtonActive = false, bool readyToInteract = false)
    {
        this.interactButtonActive = interactButtonActive;
        this.readyToInteract = readyToInteract;
    }
    public bool interactButtonActive;
    public bool readyToInteract;
}
public class PlayerInteract: UFO_PickupStuff.PickupScript
{
    private AbleToInteractStateData interactStateData = new AbleToInteractStateData();

    [SerializeField]
    private UnityEvent OnInteractWithObject = new UnityEvent();
     private bool InteractButtonActive
    {
        set
        {
            if (InteractKeyPreviousStateUp())
            {
                this.interactStateData.interactButtonActive = value;
                return;
            }

            if(value == true)
            {
                this.interactStateData.interactButtonActive = value;
                return;
            }

            this.InteractWithObject();

            this.interactStateData.interactButtonActive = value;
        }
    }

    private void Update()
    {
        int interactObject = (int)Input.GetAxis("Pickup Object");
        if(interactObject == 1)
        {
           this.InteractButtonActive = true;
        }
        else
        {
            this.InteractButtonActive = false;
        }
    }

    private void InteractWithObject()
    {
        if (!this.interactStateData.readyToInteract)
            return;
        this.OnInteractWithObject.Invoke();
    }

    private bool InteractKeyPreviousStateUp()
    {
        return this.interactStateData.interactButtonActive == false;
    }

    public void ObjectReadyToInteract()
    {
        this.interactStateData.readyToInteract = true;
    }

    public void ObjectNotReadyToInteract()
    {
        this.interactStateData.readyToInteract = false;
    }
}
