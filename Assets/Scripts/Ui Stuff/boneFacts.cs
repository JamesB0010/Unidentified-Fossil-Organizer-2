using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class boneFacts : MonoBehaviour
{
    public UnityEvent boneVoice;
    public string boneFact;
    public AudioClip alienVoice;
    private bool isClipPlayed = false;
  
    public bool isPlayed
    {
        get
        {
            return isClipPlayed;
        }

        set
        {
            isClipPlayed = value;
        }
    }

    void Start()
    {
        if(boneFact == null || boneFact == "")
        {
            boneFact = "No Fact Available";
        }
        
    }
}
