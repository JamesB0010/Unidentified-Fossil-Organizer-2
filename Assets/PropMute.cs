using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

public class SnapshotTrigger : MonoBehaviour
{
    public EventReference snapshotReference;
    private EventInstance snapshotInstance;

    void Start()
    {
        StartCoroutine(TriggerSnapshot());
    }

    IEnumerator TriggerSnapshot()
    {
        if (!snapshotReference.IsNull)
        {
            snapshotInstance = RuntimeManager.CreateInstance(snapshotReference);
            snapshotInstance.start();
            Debug.Log("muted");
            yield return new WaitForSeconds(2.5f);
            snapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            snapshotInstance.release();
            Debug.Log("unmuted");
        }
        else
        {
            Debug.LogWarning("Snapshot Reference is not assigned");
        }
    }
}
