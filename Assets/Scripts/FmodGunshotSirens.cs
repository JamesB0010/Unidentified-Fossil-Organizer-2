using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;
using FMODUnity;

public class FmodGunshotSirens : MonoBehaviour
{
    [SerializeField] private UnityEvent PlayGunshotEvent;

    private StudioGlobalParameterTrigger parameters;

    private void Awake()
    {
        this.parameters = GetComponent<StudioGlobalParameterTrigger>();
    }

    private void Start()
    {
        PlayGunshotEvent?.Invoke();
    }

    void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            this.parameters.Value = 0;
            this.parameters.TriggerParameters();
        }

        if (Input.GetKeyUp(KeyCode.CapsLock))
        {
            this.parameters.Value = 1;
            this.parameters.TriggerParameters();
        }
    }
}
