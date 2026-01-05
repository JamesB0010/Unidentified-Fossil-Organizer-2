using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenUiScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ToiletsFlushed, SinksFilled, SecretRoomsFound;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BackToMainMenu()
    {
        foreach (MusicBox musicBox in FindObjectsOfType<MusicBox>())
        {
            Destroy(musicBox.gameObject);
        }
        
        foreach (var easterEggManager in FindObjectsOfType<EasterEggManager>())
        {
            Destroy(easterEggManager.gameObject);
        }

        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    public void setGameStats(int toiletsFlushed, int sinksFilled, int secretRoomsFound)
    {
        ToiletsFlushed.text = "Toilets Flushed: " + toiletsFlushed + " / 5";
        SinksFilled.text = "Sinks Filled: " + sinksFilled + " / 3";
        SecretRoomsFound.text = "Secret Rooms Found: " + secretRoomsFound + " / 1";
    }
}
