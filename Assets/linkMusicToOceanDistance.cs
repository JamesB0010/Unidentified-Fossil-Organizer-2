using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class linkMusicToOceanDistance : MonoBehaviour
{
    [SerializeField] private Collider targetCollider;
    
    [SerializeField] private StudioEventEmitter musicEventEmitter;
    [SerializeField] private StudioEventEmitter ambienceEventEmitter;

    [SerializeField] private Transform player;

    private void Update()
    {
        Vector3 closestPoint = targetCollider.ClosestPoint(player.position);

        float distanceToCollider = Vector3.Distance(player.position, closestPoint);

        float oceanDistance = 125 - Mathf.Clamp(distanceToCollider, 0, 125);

        musicEventEmitter.SetParameter("OceanDistance", oceanDistance);
        ambienceEventEmitter.SetParameter("OceanDistance", oceanDistance);
    }
}
