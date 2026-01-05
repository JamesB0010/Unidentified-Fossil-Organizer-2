using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;
using FMOD.Studio;

public class PASystem : MonoBehaviour
{
    [SerializeField] public UnityEvent playVoice;
    [SerializeField] public float time;

    [SerializeField] private EventReference secondVoiceEvent;

    private EventInstance currentInstance;

    void Start()
    {
        StartCoroutine(Playing(time));
    }

    private IEnumerator Playing(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            // Invoke UnityEvent (assumed to play first FMOD voice)
            playVoice?.Invoke();
            Debug.Log("Invoked first voice");

            // Optional: wait a frame to let UnityEvent finish starting playback
            yield return null;

            // Try to find the EventInstance that was played (replace with your actual logic if needed)
            // If you control the FMOD play in the UnityEvent, consider wiring it here instead.
            yield return StartCoroutine(WaitForFirstToEndThenPlaySecond());
        }
    }

    private IEnumerator WaitForFirstToEndThenPlaySecond()
    {
        // Option A: You manage playback here, not via UnityEvent.
        // Option B: If UnityEvent plays it, you must obtain the instance reference some other way.
        // For this sample, we just simulate one here:
        // Replace this with actual instance if you're creating it via UnityEvent

        // ---- Example setup (delete if you're managing externally) ----
        // Simulating first voice playback here
        currentInstance = RuntimeManager.CreateInstance("event:/YourFirstEvent");
        currentInstance.start();
        currentInstance.release();
        // -------------------------------------------------------------

        PLAYBACK_STATE state;
        do
        {
            currentInstance.getPlaybackState(out state);
            yield return null;
        } while (state != PLAYBACK_STATE.STOPPED);

        Debug.Log("First voice ended, now playing second voice");

        var secondInstance = RuntimeManager.CreateInstance(secondVoiceEvent);
        secondInstance.start();
        secondInstance.release();
    }
}
