using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFO_PickupStuff{
    [ExecuteInEditMode]
public class Bone : MonoBehaviour, UFO_PickupStuff.I_Pickupable
{
    #region Attributes
    
    private ParticleSystem particleSystem;

    private bool isEnabled = true;

    [SerializeField] private string SkeletonStandBoneName;

    private bool soundEnabled = false;
    
    #endregion

    #region Methods
    private IEnumerator Start()
    {
        GetComponentReferences();
        this.particleSystem.Stop();

        yield return new WaitForSeconds(5);
        this.soundEnabled = true;
    }
    
    private void GetComponentReferences()
    {
        this.particleSystem = gameObject.GetComponent<ParticleSystem>();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        //ignore the first collision the object has because thats it just falling to the ground when the game starts
        if (!this.soundEnabled)
        {
            return;
        }
        this.particleSystem.Play();
    }
    
    
    public string GetSkeletonStandBoneName()
    {
        return SkeletonStandBoneName;
    }

    public bool IsEnabled
    {
        get
        {
            return this.isEnabled;
        }
        set
        {
            this.isEnabled = value;
        }
    }

    private void OnValidate()
    {
        MeshCollider meshCollider = this.GetComponent<MeshCollider>();

        meshCollider.sharedMesh = this.GetComponent<MeshFilter>().sharedMesh;
    }

}

#endregion
}
