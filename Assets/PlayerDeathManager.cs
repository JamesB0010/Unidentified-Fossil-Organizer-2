using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;

public class PlayerDeathManager : MonoBehaviour
{
    public GameObject[] paSpeakers;

    public void ReactToDeathStarted()
    {
        this.StopAllPASounds();
    }
    
    private void StopAllPASounds()
    {
        foreach (GameObject pa in paSpeakers)
        {
            pa.GetComponentInChildren<newPAsystem>().enabled = false;
            pa.GetComponent<StudioEventEmitter>().Stop();
            pa.GetComponentsInChildren<StudioEventEmitter>().ToList().ForEach(emitter => emitter.Stop());
        }
    }
}
