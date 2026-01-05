using System;
using System.Collections;
using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;
using UnityEngine.Events;

public class BananaPeel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Gocart"))
        {
            other.gameObject.GetComponentInParent<ArcadeKart>().SkidOutOfControl();
        }
    }
}
