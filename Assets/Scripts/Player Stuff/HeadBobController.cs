using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;
using FMOD.Studio;
//credit for headbob https://www.youtube.com/watch?v=5MbR2qJK8Tc for tutorial
//adapted by James
public class HeadBobController : MonoBehaviour
{
    public UnityEvent playfootsteps;
    public UnityEvent stopfootsteps;
    public StudioEventEmitter emitter; 
    public string parameterName;
    
    [SerializeField] private bool enable = true;

    private bool walking = false;
    [SerializeField, Range(0, 10f)] private float amplitude = 0.002f;
    [SerializeField, Range(0, 300)] private float frequency = 10.0f;
    [SerializeField] private float sideBobAmplitude = 0.2f;

    private bool resetCamera = false;
    [SerializeField] private Transform camera = null;
    [SerializeField] private Transform cameraHolder = null;
    
    private Vector3 startPos;

    private void Awake()
    {
        this.startPos = this.camera.localPosition;
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * Mathf.PI * 2 * frequency) * amplitude * Time.deltaTime;
        pos.x += Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude * 0.2f * Time.deltaTime;
        return pos;
    }

    private void ResetPosition()
    {
        if (this.camera.localPosition == this.startPos)
        {
            this.resetCamera = false;
            return;
        }
        this.camera.localPosition = Vector3.Lerp(this.camera.localPosition, this.startPos, 3 * Time.deltaTime);
    }

    [SerializeField]
    private bool footstepSoundEnabled = true;

    public void SetFootstepSoundEnabled(bool value)
    {
        this.footstepSoundEnabled = value;
    }
    
    public void StartWalking()
    {
        this.walking = true;
        this.resetCamera = false;
        
        if(this.footstepSoundEnabled)
            playfootsteps?.Invoke();
    }

    public void StopWalking()
    {
        this.walking = false;
        this.resetCamera = true;
        this.ResetPosition();
        stopfootsteps?.Invoke();
    }

    private void PlayMotion(Vector3 motion)
    {
        this.camera.localPosition += motion;
    }
    private void Update()
    {
        if (!this.enable)
            return;

        if(this.resetCamera)
            ResetPosition();
        
        if (!this.walking)
            return;
        
        this.PlayMotion(this.FootStepMotion());
    }
}
