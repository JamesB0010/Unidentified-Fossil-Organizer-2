using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;
using FMOD.Studio;

public class checkfloor : MonoBehaviour
{
    public StudioEventEmitter emitter;
    public string parameterName;
    public float lastfloor;
    public UnityEvent InBathroomReverb;
    public UnityEvent OutBathroomReverb;

    [SerializeField] private Transform cameraTransform;

    private string currentWaterState = "None";  // Tracks the current state

    private void Start()
    {
        if (emitter == null)
        {
            emitter = GetComponent<StudioEventEmitter>();
        }

        StartCoroutine(PrintHeightEverySecond());
    }

    private void OnCollisionStay(Collision other)
    {
        // Handle wood collision
        if (other.gameObject.CompareTag("wood"))
        {
            SetParameter(1.5f);
            lastfloor = 1.5f;
            OutBathroomReverb?.Invoke();
            currentWaterState = "Dry";
            Debug.Log("Wood");
        }
        // Handle tiles collision
        else if (other.gameObject.CompareTag("tiles"))
        {
            SetParameter(2.5f);
            lastfloor = 2.5f;
            InBathroomReverb?.Invoke();
            currentWaterState = "Dry";
            Debug.Log("Tiles");
        }
        // Handle sand collision
        else if (other.gameObject.CompareTag("sand"))
        {
            float cameraHeight = cameraTransform.position.y;

            // Underwater
            if (cameraHeight < 4.405f && currentWaterState != "Underwater")
            {
                SetParameter(5.5f);
                lastfloor = 5.5f;
                InBathroomReverb?.Invoke();
                currentWaterState = "Underwater";
                Debug.Log("Underwater");
            }
            // Wading
            else if (cameraHeight >= 4.405f && cameraHeight < 5.3f && currentWaterState != "Wading")
            {
                SetParameter(4.5f);
                lastfloor = 4.5f;
                OutBathroomReverb?.Invoke();
                currentWaterState = "Wading";
                Debug.Log("Wading");
            }
            // Shallows
            else if (cameraHeight >= 5.3f && cameraHeight < 5.85f && currentWaterState != "Shallows")
            {
                SetParameter(3.5f);
                lastfloor = 3.5f;
                OutBathroomReverb?.Invoke();
                currentWaterState = "Shallows";
                Debug.Log("Shallows");
            }
            // Sand
            else if (cameraHeight >= 5.85f && currentWaterState != "Sand")
            {
                SetParameter(0.5f);
                lastfloor = 0.5f;
                OutBathroomReverb?.Invoke();
                currentWaterState = "Sand";
                Debug.Log("Sand");
            }
        }
        // Default
        else
        {
            SetParameter(lastfloor); // Keep previous surface sound
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("end"))
        {
            OutBathroomReverb?.Invoke();
            Debug.Log("Forced OutBathroomReverb triggered by trigger zone.");
        }
    }

    public void SetParameter(float value)
    {
        if (emitter != null)
        {
            emitter.SetParameter(parameterName, value);
        }
        else
        {
            Debug.LogWarning("FMOD Studio Event Emitter not found!");
        }
    }

    private IEnumerator PrintHeightEverySecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (Camera.main != null)
            {
                float cameraHeight = cameraTransform.position.y;
                Debug.Log("Camera Height: " + cameraHeight);
            }
            else
            {
                Debug.LogWarning("Main Camera not found!");
            }
        }
    }
}
