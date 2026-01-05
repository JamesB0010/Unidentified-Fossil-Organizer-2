using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GocartHitDetector : MonoBehaviour
{
    [SerializeField] private UnityEvent gocartCollisionEnter;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Gocart"))
        {
            this.gocartCollisionEnter?.Invoke();
        }
    }
}
