using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BeachToEndScreenLink : MonoBehaviour
{
    public UnityEvent EndScreenLinkExecute;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EndScreenLinkExecute?.Invoke();
            SceneManager.LoadSceneAsync("EndScreen");
        }
    }
}
