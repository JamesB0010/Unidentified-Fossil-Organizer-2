using System.Collections;
using System.Collections.Generic;
using UFO_PlayerStuff;
using UnityEngine;

public interface I_Interactable
{
    public abstract void HandleInteraction(CameraForwardsSampler playerCamSampler);
}
