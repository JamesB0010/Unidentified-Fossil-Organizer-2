using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArmUpDown : MonoBehaviour
{
    [SerializeField] private Transform armUpPos;

    [SerializeField] private Transform armDownPos;

    private bool armMoving = false;

    private void Start()
    {
        this.ArmDown();
    }

    public void ArmUp()
    {
        /*if (this.armMoving == true)
            return;
        this.armMoving = true;
        armDownPos.position.LerpTo(armUpPos.position, 1.0f, 
            value =>
            {
                transform.position = value;
            },
            pkg =>
            {
                Debug.Log("Arm up");
                this.armMoving = false;
            });*/
    }

    public void ArmDown()
    {
        /*if (this.armMoving == true)
            return;
        this.armMoving = true;
        armUpPos.position.LerpTo(armDownPos.position, 1.0f, value =>
        {
            transform.position = value;
        },
            pkg =>
            {
                Debug.Log("Arm down");
                this.armMoving = false;
            });*/
    }
}
