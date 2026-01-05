using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UFO_PickupStuff;
using UFO_PlayerStuff;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Internal;
using UnityEngine.Serialization;

public class SkeletonStand : MonoBehaviour, I_Interactable
{
    [SerializeField] private StudioEventEmitter boneSwooshSound;
    //Bone positional Data
    [SerializeField] 
    private List<GameObject> skeletonBones = new List<GameObject>();
    private Dictionary<string, Transform> boneNameTransforms = new Dictionary<string, Transform>();

    [SerializeField] private ParticleSystem yipeeParticle;

    private PickupScript playerPickupScript;
    
    //Progress data

    [SerializeField] 
    private UnityEvent allBonesCollected = new UnityEvent();
    private int bonesNeededToWin;

    public int bonesDelivered = 0;

    private int BonesDelivered
    {
        get
        {
            return this.bonesDelivered;
        }

        set
        {
            this.bonesDelivered = value;
            bool allBonesDisabled = true;
            foreach (Bone skeletonBone in FindObjectsOfType<Bone>())
            {
                if (skeletonBone.IsEnabled == false)
                {
                    allBonesDisabled = false;
                    break;
                }
            }
            if (this.bonesDelivered == this.bonesNeededToWin && !allBonesDisabled)
            {
                this.allBonesCollected.Invoke();
                PopConfetti();
            }
            
        }
    }
    
    private void Start()
    {
        this.bonesNeededToWin = this.skeletonBones.Count;
        this.playerPickupScript = FindObjectOfType<PickupScript>();
        PopulateBoneNameTransforms();
    }

    public void PopConfetti()
    {
        this.yipeeParticle.Play();
    }

    private void PopulateBoneNameTransforms()
    {
        foreach (GameObject bone in this.skeletonBones)
        {
            this.boneNameTransforms.Add(bone.name, bone.transform);
        }
    }

    public void HandleInteraction(CameraForwardsSampler playerCamSampler)
    {
        if (playerCamSampler.InteractableObjectInRangeRef != this.gameObject)
            return;
        //Get a reference to the object the player is holding
        GameObject bone = playerCamSampler.ObjectInRange;

        boneSwooshSound.Play();
        
        Debug.Log(this.playerPickupScript.DroppedObjectTimestamp - Time.time > 0.1f);
        
        if(bone == null)
            return;
        
        //if the player is not holding a bone then return
        if (!bone.TryGetComponent(out Bone BoneCastObj))
            return;
        
        AddNewLerpPackageToPkgQueue(bone, BoneCastObj);
        bone.GetComponent<ParticleSystem>().enableEmission = false;
    }

    private void AddNewLerpPackageToPkgQueue(GameObject bone, Bone BoneCastObj)
    {
        bone.transform.position.LerpTo(boneNameTransforms[BoneCastObj.GetSkeletonStandBoneName()].position, 
            1,
            value => { bone.transform.position = value;}
            );
        
        bone.transform.rotation.eulerAngles.LerpTo(boneNameTransforms[BoneCastObj.GetSkeletonStandBoneName()].rotation.eulerAngles, 
            1, 
            value =>{bone.transform.rotation = Quaternion.Euler(value);},
            pkg =>
            {
                BoneCastObj.GetComponent<Rigidbody>().isKinematic = true;
                BoneCastObj.IsEnabled = false;
                this.BonesDelivered++;
            });
    }
}