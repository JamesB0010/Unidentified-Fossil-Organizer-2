using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedUnityEvent : MonoBehaviour
{
    public float delayTime;

    public UnityEvent e;

    public void Go()
    {
        StartCoroutine(nameof(this.delay));
    }

    private IEnumerator delay()
    {
        yield return new WaitForSeconds(delayTime);
        e?.Invoke();
    }
}
