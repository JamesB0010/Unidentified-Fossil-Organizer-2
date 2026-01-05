using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventDrivenIntensity))]
public class EventDrivenIntensityEditor : Editor
{
    private EventDrivenIntensity t;

    private void OnEnable()
    {
        this.t = target as EventDrivenIntensity;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Max intensity"))
        {
            this.t.IncrementIntensity(2);
        }
    }
}

