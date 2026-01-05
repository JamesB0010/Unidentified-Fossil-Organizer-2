using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class MusicBox : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;

    private int currentClip;

    private static MusicBox musicBoxReference = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (MusicBox.musicBoxReference == null)
            MusicBox.musicBoxReference = this;
        else
            Destroy(this.gameObject);
        
        
        DontDestroyOnLoad(this.gameObject);
        
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    private void Start()
    {
        //select a random song
        this.currentClip = (int)Mathf.Floor(UnityEngine.Random.Range(0, 2));
    }
}
