using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private string[] busNames;

    private Bus[] busses;

    private void Awake()
    {
        this.busses = new Bus[busNames.Length];
        
        for (int i = 0; i < busNames.Length; i++)
        {
            busses[i] = RuntimeManager.GetBus(this.busNames[i]);
        }
    }

    public void OnSliderValChanged(float value)
    {
        this.UpdateBussesVolume(value);
    }


    private void UpdateBussesVolume(float volume)
    {
        foreach (Bus bus in busses)
        {
            bus.setVolume(volume);
        }
    }
}
