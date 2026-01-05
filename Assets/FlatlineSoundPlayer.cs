using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class FlatlineSoundPlayer : MonoBehaviour
{
    private static EventInstance flatlineEventInstance;

    private void Awake()
    {
        flatlineEventInstance = RuntimeManager.CreateInstance("event:/Foley/Deaths/Flatline");
    }

    public void Play()
    {
        flatlineEventInstance.start();
    }

    public static void Stop()
    {
        flatlineEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
        flatlineEventInstance.release();
    }
}
