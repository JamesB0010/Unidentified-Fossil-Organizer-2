using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEditor;
using UnityEngine;

public class EventDrivenIntensity : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter emitter;
    private float intensity;

    public float Intensity
    {
        get => this.intensity;
        set
        {
            this.intensity = Mathf.Clamp01(value);
        }
    }

    [SerializeField] private float falloffScalar;

    private void Update()
    {
        Intensity -= Time.deltaTime * falloffScalar;

        this.emitter.SetParameter("Activity", Intensity);
        //Debug.Log(Intensity);
    }

    public void IncrementIntensity(float amountToIncrement)
    {
        this.Intensity += amountToIncrement;
    }
}

