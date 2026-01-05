using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PABreak : MonoBehaviour
{
    public Rigidbody rigidbody;
    public bool hasfallen = false;
    public UnityEvent smash;
    private void OnCollisionEnter(Collision collision)
    {
        
        if (rigidbody != null && !rigidbody.useGravity)
        {
            smash.Invoke();
            rigidbody.useGravity = true;
            Debug.Log("Gravity enabled on Rigidbody.");
        }
        
    }
}
