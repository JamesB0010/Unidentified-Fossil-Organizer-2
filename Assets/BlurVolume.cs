using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BlurVolume : VolumeComponent
{
    [SerializeField] private FloatParameter blurMag = new FloatParameter(0);

    [SerializeField] private BoolParameter enabled = new BoolParameter(true);

    public float BlurMag
    {
        get => this.blurMag.value;
        set => this.blurMag.value = value;
    }

    public bool Enabled
    {
        get => this.enabled.value; 
        set => this.enabled.value = value;
    }
}
