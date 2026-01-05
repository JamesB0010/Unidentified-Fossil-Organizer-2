using System;
using System.Collections;
using System.Collections.Generic;
using UFO_PlayerStuff;
using UnityEngine;
using UFO_PickupStuff;

public class AimAssist : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHeadVerticalMovement playerHeadVerticalMovement;
    private CameraForwardsSampler cameraForwardsSampler;
    private PickupScript pickupScript;

    private float defaultHorizontalSensitivity;
    private float defaultVerticalSensitivity;
    private void Start()
    {
        this.playerMovement = FindObjectOfType<PlayerMovement>();
        this.playerHeadVerticalMovement = FindObjectOfType<PlayerHeadVerticalMovement>();
        this.cameraForwardsSampler = FindObjectOfType<CameraForwardsSampler>();
        this.pickupScript = FindObjectOfType<PickupScript>();
        CameraForwardsSampler cameraForwardSampler = FindObjectOfType<CameraForwardsSampler>();
        cameraForwardSampler.PickupableObjectInRangeUnityEvent.AddListener(this.OnLookAtPickupable);
        cameraForwardSampler.PickupableObjectOutOfRangeUnityEvent.AddListener(this.StopLookingAtPickupable);

        this.defaultHorizontalSensitivity = playerMovement.RotationSpeed;
        this.defaultVerticalSensitivity = playerHeadVerticalMovement.rotationSensitivity;
    }

    private void OnLookAtPickupable()
    {
        if (this.cameraForwardsSampler.ObjectInRange.TryGetComponent(out Bone bone))
        {
            if (bone.enabled == false)
                return;
            this.playerMovement.RotationSpeed = defaultHorizontalSensitivity * 0.125f;
            this.playerHeadVerticalMovement.rotationSensitivity = defaultVerticalSensitivity * 0.125f;
        }
    }

    private void StopLookingAtPickupable()
    {
        if (this.cameraForwardsSampler.ObjectInRange.TryGetComponent(out Bone bone))
        {
            if (bone.enabled == false)
                return;
            this.playerMovement.RotationSpeed = this.defaultHorizontalSensitivity;
            this.playerHeadVerticalMovement.rotationSensitivity = this.defaultVerticalSensitivity;
        }
    }
}