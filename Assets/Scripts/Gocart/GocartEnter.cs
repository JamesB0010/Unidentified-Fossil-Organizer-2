using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using KartGame.KartSystems;
using UFO_PlayerStuff;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using FMODUnity;

public class GocartEnter : MonoBehaviour, I_Interactable
{
    private KeyboardInput gocartKeyboardInput;

    [SerializeField] private GameObject playerGocartModel;

    [SerializeField] private UnityEvent StartedToEnterGoCart;

    [SerializeField] private UnityEvent ScreenBlackDuringEnter;
    
    [SerializeField] private UnityEvent StartedToExitGoCart;
    
    [SerializeField] private UnityEvent ScreenBlackDuringExit;

    [SerializeField] private GameObject goCartCamera;

    private float yValueToReturnPlayerTo = 2.699108f;
    
    private GameObject playerGameObject;

    private bool exitingGoCart = false;
    
    private FMODUnity.StudioEventEmitter kartEmitter;

    private void Awake()
    {
        this.gocartKeyboardInput = GetComponent<KeyboardInput>();
        this.gocartKeyboardInput.enabled = false;

        this.playerGocartModel.SetActive(false);

        kartEmitter = GetComponentInChildren<StudioEventEmitter>();
    }

    public void HandleInteraction(CameraForwardsSampler playerCamSampler)
    {
        if (playerCamSampler.InteractableObjectInRangeRef != this.gameObject)
            return;
        
        this.StartedToEnterGoCart?.Invoke();

        StartCoroutine(this.EnterGOcart(playerCamSampler.gameObject));
    }

    public IEnumerator EnterGOcart(GameObject playerCamSampler)
    {
        if (this.playerGameObject == null)
            this.playerGameObject = playerCamSampler.GetComponentInParent<PlayerMovement>().gameObject;

        yield return new WaitForSeconds(0.2f);
        this.goCartCamera.SetActive(true);
        playerGocartModel.SetActive(true);
        this.gocartKeyboardInput.enabled = true;
        this.playerGameObject.SetActive(false);
        this.ScreenBlackDuringEnter?.Invoke();
        Debug.Log("Vroom Vrrom");
    }

    private IEnumerator ExitGoCart()
    {
        yield return new WaitForSeconds(0.2f);
        this.playerGameObject.SetActive(true);
        Vector3 playerPos = transform.position;
        playerPos.y = this.yValueToReturnPlayerTo;
        this.playerGameObject.transform.position = playerPos;
        this.ScreenBlackDuringExit?.Invoke();
        this.goCartCamera.SetActive(false);
        playerGocartModel.SetActive(false);
        this.gocartKeyboardInput.enabled = false;
        Debug.Log("Byeee");

        this.exitingGoCart = false;
    }

    private void Update()
    {
        if (Input.GetAxis("Jump") == 1 && this.gocartKeyboardInput.enabled && !this.exitingGoCart)
        {
            this.exitingGoCart = true;
            this.StartedToExitGoCart?.Invoke();

            StartCoroutine(this.ExitGoCart());
            
        }
    }
}