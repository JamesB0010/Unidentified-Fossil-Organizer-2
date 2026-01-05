using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class EasterEggManager : MonoBehaviour
{
    private DoctorsSink[] Sinks;

    private bool shuttersOpen = false;

    public int fullSinkCount = 0;

    public int toiletsFlushed = 0;

    private List<string> toiletsFlushedNames = new List<string>();

    public int sinksFilled = 0;

    private List<string> filledSinksNames = new List<string>();

    public int secretRoomsFound = 0;

    public UnityEvent OpenShuttersEvent = new UnityEvent();

    public UnityEvent CloseShuttersEvent = new UnityEvent();
    
    public UnityEvent stopSoundEvent = new UnityEvent();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += this.onSceneChanged;
        this.Sinks = FindObjectsOfType<DoctorsSink>();

        foreach (DoctorsSink sink in this.Sinks)
        {
            sink.OnSinkFull += ReactToSinkFull;
            sink.OnSinkEmpty += ReactToSinkEmpty;

        }
    }

    private void onSceneChanged(Scene old, Scene newscene)
    {
        EndScreenUiScript endScreen = FindObjectOfType<EndScreenUiScript>(); endScreen.setGameStats(this.toiletsFlushed, this.sinksFilled, this.secretRoomsFound);
    }

    public void OnDestroy()
    {
        SceneManager.activeSceneChanged -= this.onSceneChanged;
    }

    public void reactToToiletFlushed(string toiletName)
    {
        if (this.toiletsFlushedNames.Contains(toiletName))
        {
            return;
        }
        else
        {
            Debug.Log("Toilet flushed" + toiletName);
            this.toiletsFlushed++;
            this.toiletsFlushedNames.Add(toiletName);
        }
    }

    public void ReactToSinkFull(string sinkName)
    {
        if (this.filledSinksNames.Contains(sinkName))
        {
        }
        else
        {
            Debug.Log("sink filled");
            this.sinksFilled++;
            this.filledSinksNames.Add(sinkName);
        }
        this.fullSinkCount++;
        this.sinksFilled = this.sinksFilled < 3? sinksFilled + 1: 3;
        if(this.fullSinkCount >= this.Sinks.Length)
            this.AllSinksFull();
    }

    public void ReactToSinkEmpty(string sinkName)
    {
        this.fullSinkCount--;
        if (this.shuttersOpen)
            this.CloseShutters();
    }

    private void AllSinksFull()
    {
        this.OpenShuttersEvent.Invoke();
        this.shuttersOpen = true;
    }

    private void CloseShutters()
    {
        this.shuttersOpen = false;
        this.CloseShuttersEvent.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        this.secretRoomsFound = 1;

        GetComponent<BoxCollider>().enabled = false;
    }
}
