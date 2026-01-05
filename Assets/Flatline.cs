using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class SimpleSnapshotFader : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter snapshotEmitter;

    void Start()
    {
        if (snapshotEmitter == null)
        {
            Debug.LogError("No snapshot emitter assigned.");
            return;
        }

        snapshotEmitter.Play();
        Debug.Log("Starting snapshot fade...");

        StartCoroutine(FadeParameter(snapshotEmitter.EventInstance, "Flatline", 6f));
    }

    private IEnumerator FadeParameter(EventInstance instance, string parameter, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            float value = time / duration;
            instance.setParameterByName(parameter, value);
            time += Time.deltaTime;
            yield return null;
        }

        instance.setParameterByName(parameter, 1f);
        FlatlineSoundPlayer.Stop();
        Debug.Log("Snapshot fade complete.");
    }
}
