using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Shutter : MonoBehaviour
{
    private Animator animator;
    public UnityEvent onCompleteShutter;
    public UnityEvent onNotCompleteShutter;
    [SerializeField] private BoxCollider BoxCollider;

    private void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    public void OpenShutters()
    {
        if (this.animator == null)
            return;
        
        this.animator.SetTrigger("Open");
        this.BoxCollider.enabled = false;
        onNotCompleteShutter.Invoke();
        StartCoroutine(WaitForAnimation("Open", () =>
        {
            Debug.Log("Shutters finished opening!");
            onCompleteShutter.Invoke();
        }));
    }

    public void CloseShutters()
    {
        if (this.animator == null)
            return;
        
        this.animator.SetTrigger("Close");
        this.BoxCollider.enabled = true;
        onNotCompleteShutter.Invoke();
        StartCoroutine(WaitForAnimation("Close", () =>
        {
            Debug.Log("Shutters finished opening!");
            onCompleteShutter.Invoke();
        }));
    }
    
    private IEnumerator WaitForAnimation(string stateName, System.Action onCompleteShutter)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    
        
        while (!stateInfo.IsName(stateName))
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

      
        while (stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        onCompleteShutter?.Invoke();
    }
}
