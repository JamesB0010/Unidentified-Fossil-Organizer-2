using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using UFO_PlayerStuff;
using UnityEngine.UI;
using System;
using FMODUnity;
using UnityEngine.Events;

public class TextLookTowards : MonoBehaviour
{
    private GameObject playerCam;
    private GameObject parentBone = null;

    private CameraForwardsSampler cameraSampler;

    private bool isPlayerHoldingBone = false;

    public TMP_Text boneText;

    [SerializeField] private float rightOffset = 0.5f;

    [SerializeField] private float forwardsOffset = 1;

    private MusicBox musicBox;

    [SerializeField] private UnityEvent bonePickedUp;

    [SerializeField] private UnityEvent PlayAlienVoice;

    [SerializeField] private UnityEvent boneDropped;

    private int bonesPickedUp = 0;

    [SerializeField] private UnityEvent FirstBonePickedUp;

    private IEnumerator triggerFirstBonePickedUpEvent()
    {
        parentBone = cameraSampler.ObjectInRange;
        Debug.Log(parentBone);
        isPlayerHoldingBone = true;
        Debug.Log("" + parentBone.GetComponent<boneFacts>().isPlayed + " " + isPlayerHoldingBone);
        this.FirstBonePickedUp?.Invoke();
        this.bonePickedUp?.Invoke();
        
        yield return new WaitForSeconds(4);
        if (this.isPlayerHoldingBone)
        {
            if (!parentBone.GetComponent<boneFacts>().isPlayed && isPlayerHoldingBone)
            {
                parentBone.GetComponent<boneFacts>().isPlayed = true;
                Debug.Log("Player pickup bone success");
                var bonecheck = cameraSampler.ObjectInRange.GetComponents<StudioEventEmitter>();
                bonecheck[1].Play();
                this.PlayAlienVoice?.Invoke();
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        this.musicBox = FindObjectOfType<MusicBox>();
        playerCam = GameObject.FindGameObjectWithTag("MainCamera");

        cameraSampler = GameObject.FindAnyObjectByType<CameraForwardsSampler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (parentBone != null && isPlayerHoldingBone)
        {
            TextLookAt();
            followParent();
        }
    }
    private void followParent()
    {
        string temp = parentBone.GetComponent<boneFacts>().boneFact;
        Debug.Log(temp);
        boneText.text = "" + temp;
        this.transform.position = new Vector3(parentBone.transform.position.x, parentBone.transform.position.y + 0.3f, parentBone.transform.position.z) + (new Vector3(playerCam.transform.right.x, playerCam.transform.right.y, playerCam.transform.right.z) * this.rightOffset) + (new Vector3(playerCam.transform.forward.x, playerCam.transform.forward.y, playerCam.transform.forward.z) * this.forwardsOffset);
    }
    private void TextLookAt()
    {
        this.transform.LookAt(playerCam.transform.position + playerCam.transform.rotation * Vector3.forward, playerCam.transform.rotation * Vector3.up);
    
        Quaternion q = this.transform.rotation;
        this.transform.rotation = Quaternion.Euler(0, q.eulerAngles.y + 180, 0);
    }

    public void OnPickupBone()
    {
        try
        {
            if (this.bonesPickedUp == 0 && cameraSampler.ObjectInRange.TryGetComponent(out boneFacts b))
            {
                this.bonesPickedUp++;

                StartCoroutine(nameof(this.triggerFirstBonePickedUpEvent));
                return;
            }
            this.bonePickedUp?.Invoke();
            parentBone = cameraSampler.ObjectInRange;
            Debug.Log(parentBone);
            isPlayerHoldingBone = true;
            Debug.Log("" + parentBone.GetComponent<boneFacts>().isPlayed + " " + isPlayerHoldingBone);
            if (!parentBone.GetComponent<boneFacts>().isPlayed && isPlayerHoldingBone)
            {
                Debug.Log("throught the if, " + parentBone.GetComponent<boneFacts>().isPlayed + " " + isPlayerHoldingBone);
                
                parentBone.GetComponent<boneFacts>().isPlayed = true;
                Debug.Log("Player pickup bone success");
                var bonecheck = cameraSampler.ObjectInRange.GetComponents<StudioEventEmitter>();
                bonecheck[1].Play();
                this.PlayAlienVoice?.Invoke();
            }
        } catch (NullReferenceException error) { isPlayerHoldingBone = false; }

    }

    public void OnDropBone()
    {
        parentBone = null;
        boneText.text = " ";
        this.boneDropped?.Invoke();
        this.isPlayerHoldingBone = false;
    }
}