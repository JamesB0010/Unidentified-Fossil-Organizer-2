using System.Collections;
using System.Collections.Generic;
using UFO_PickupStuff;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(MeshCollider))]
public class GenericPickupableObject : MonoBehaviour, UFO_PickupStuff.I_Pickupable
{
    void Start()
    {
        gameObject.GetComponent<MeshCollider>().convex = true;
    }
}
