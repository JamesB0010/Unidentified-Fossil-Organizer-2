using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimBP : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    public void ReactToStartedWalking()
    {
        this.animator.SetTrigger("StartWalk");
    }

    public void ReactToStopWalking()
    {
        this.animator.SetTrigger("StopWalk");
    }
}
