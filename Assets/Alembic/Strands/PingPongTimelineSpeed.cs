using System;
using UnityEngine;
using UnityEngine.Playables;

//chatGpt
public class PingPongTimelineSpeed : MonoBehaviour
{
    public PlayableDirector director;
    private bool playingForward = true;

    void Update()
    {
        if (director == null) return;

        // Check if we've reached the end (or the start)
        if (playingForward && director.time >= director.duration)
        {
            playingForward = false;
        }
        else if (!playingForward && director.time <= 0)
        {
            playingForward = true;
        }

        // Play forward or backward
        director.time += (playingForward ? Time.deltaTime * 0.5 : -Time.deltaTime * 0.5);
        director.Evaluate(); // Force update without stopping the timeline
    }
}