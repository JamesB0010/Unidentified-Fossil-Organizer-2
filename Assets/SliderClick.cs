using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class SliderClickAudio : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private EventReference fmodClickEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!fmodClickEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(fmodClickEvent);
        }
    }
}
