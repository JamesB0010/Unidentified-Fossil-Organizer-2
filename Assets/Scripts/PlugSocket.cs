using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UFO_PickupStuff;
using UFO_PlayerStuff;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlugSocket : MonoBehaviour
{
    [SerializeField] private PickupScript pickupScript;

    [SerializeField] private PlayerMovement playerMovement;
    
    [SerializeField] private VideoPlayer shockVideoPlayer;

    [SerializeField] private GameObject shockImage;

    [SerializeField] private GameObject whiteSceenImage;

    [SerializeField] private PlayerHeadVerticalMovement playerVerticalLook;
    
    [SerializeField] private UnityEvent startedKillingPlayer;

    private bool killingPlayer = false;
    
    private const int beachSceneIndex = 4;

    private async void OnCollisionEnter(Collision other)
    {
        if (killingPlayer)
            return;
        killingPlayer = true;
        bool tagMatches = other.collider.CompareTag("Fork");
        bool isHolding = pickupScript.IsHolding();
        if (tagMatches && isHolding)
        {
            this.startedKillingPlayer?.Invoke();
            playerMovement.DisableWalkSpeed();
            this.playerVerticalLook.enabled = false;

            GetComponent<Collider>().enabled = false;
            this.shockVideoPlayer.Play();
            await UniTask.DelayFrame(3);
            playerMovement.enabled = false;
            this.shockImage.SetActive(true);

            var handle = SceneManager.LoadSceneAsync(beachSceneIndex);
            handle.allowSceneActivation = false;
            
            await UniTask.Delay(TimeSpan.FromSeconds(5.6f));
            this.whiteSceenImage.SetActive(true);

            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));

            handle.allowSceneActivation = true;
        }
    }
}
