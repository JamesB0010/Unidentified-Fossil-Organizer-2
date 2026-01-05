using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;
public class newPAsystem : MonoBehaviour
{
    public Rigidbody rigidbody;
    [SerializeField]
    public UnityEvent playVoice;
    public UnityEvent playStatic;

    [SerializeField] public float time;
    void Start()
    {
        StartCoroutine(Playing(time));
    }

    
    void Update()
    {
        
       
    }

    private IEnumerator Playing(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            if (rigidbody.useGravity == true)
            {
                if (this.enabled)
                {
                    playStatic?.Invoke();
                    Debug.Log("Playing Static");
                }
            }
            else
            {
                if (this.enabled)
                {
                    Debug.Log("we have changed it to " + PAParam.selectionIndex + " lollll");
                    RuntimeManager.StudioSystem.setParameterByName("PASelection", PAParam.selectionIndex);
                    playVoice?.Invoke();
                    Debug.Log("Playing Voice");
                }
            }
          
            Debug.Log("we invoking with this one");
        }
    }
}
