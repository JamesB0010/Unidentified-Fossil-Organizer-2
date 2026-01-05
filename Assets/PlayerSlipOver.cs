using System;
using System.Collections;
using System.Collections.Generic;
using UFO_PlayerStuff;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSlipOver : MonoBehaviour
{
    [SerializeField] private PlayableDirector slipOverAnimation;

    [SerializeField] private PlayerHeadVerticalMovement verticalHeadMovement;
    
    [SerializeField] private Image whiteScreenCover;

    [SerializeField] private Volume postProcessVolume;
    private const int beachSceneIndex = 4;
    
    private void OnTriggerEnter(Collider other)
    {
        if(!base.enabled)
            return;
        
        if (other.CompareTag("SlipOver"))
        {
            Debug.Log("Slip over");
            base.enabled = false;
            this.slipOverAnimation.Play();
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<HeadBobController>().enabled = false;
            verticalHeadMovement.enabled = false;
        }
    }

    public void StartFadingToWhite()
    {
        StartCoroutine(nameof(this.FadeWhiteIn));
    }

    private IEnumerator FadeWhiteIn()
    {
        this.whiteScreenCover.gameObject.SetActive(true);

        this.whiteScreenCover.color = new Color(1, 1, 1, 0);

        this.whiteScreenCover.color.LerpTo(Color.white, 3f, val => this.whiteScreenCover.color = val, pkg =>
        {
                StartCoroutine(nameof(this.ChangeSceneAfter));
        }, null);

        if (this.postProcessVolume.profile.TryGet(out BlurVolume blurVol))
        {
            blurVol.Enabled = true;
            0.0f.LerpTo(60f, 1.5f, val => blurVol.BlurMag = val, null, null);
        }
        yield return null;
    }

    private IEnumerator ChangeSceneAfter()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync(beachSceneIndex);
    }
}
