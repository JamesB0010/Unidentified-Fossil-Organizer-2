using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace  UFO_PlayerStuff{
    
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private float walkSpeed = 5f;

    public void DisableWalkSpeed() => walkSpeed = 0;
    
    [SerializeField] private float rotationSpeed = 5f;
    public float RotationSpeed
        {
            get => rotationSpeed;

            set => rotationSpeed = value;
        }
    private Rigidbody rigidbody;

    [SerializeField] 
    private UnityEvent StartedWalking = new UnityEvent();

    [SerializeField] 
    private UnityEvent StoppedWalking = new UnityEvent();
    
    private bool isWalking = false;
    public bool IsWalking
    {
        set
        {
            //if nothing has changed then oh well
            if (isWalking == value)
                return;
            
            //stopped walking
            if (value == false)
            {
                this.StoppedWalking.Invoke();
                isWalking = value;
            }

            if (value == true)
            {
                this.StartedWalking?.Invoke();
                isWalking = value;
            }
        }
    }

    //MonoBehaviour Method
    private void Start()
    {
        this.rigidbody = GetComponent<Rigidbody>();

        ConfigureMouse();
    }

    private static void ConfigureMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        SetPlayerVelocityDrivenByInput();
        SetPlayerYRotationDrivenByMouseInput();
    }

    private float arrowValue = 0.0f;
    private void SetPlayerYRotationDrivenByMouseInput()
    {
        float tempRotSpeed = this.rotationSpeed;
        if (isWalking == true)
        {
            tempRotSpeed *= 2;
        }
        float mouseX = Input.GetAxis("Mouse X") * this.rotationSpeed;/* * Time.deltaTime;*/
        this.rigidbody.MoveRotation(this.rigidbody.rotation * Quaternion.Euler(0, mouseX, 0));

        float arrowXPositive = Input.GetKey("right") == true? (0.5f * this.rotationSpeed) / 4: 0;
        float arrowXNegative = Input.GetKey("left") == true ? (-0.5f * this.rotationSpeed) / 4 : 0;
        arrowValue += (arrowXPositive + arrowXNegative);
        
        this.rigidbody.MoveRotation(this.rigidbody.rotation * Quaternion.Euler(0, arrowValue, 0));
        arrowValue = 0.0f;
    }
    

    private void SetPlayerVelocityDrivenByInput()
    {
        float currentYVelocity = this.rigidbody.velocity.y;
        float leftRightInput = Input.GetAxis("Horizontal");
        float forwardsBackInput = Input.GetAxis("Vertical");

        Vector3 velocityForward = new Vector3();
        velocityForward = CalculateInputtedForwardVelocity(forwardsBackInput);

        Vector3 velocityRight = CalculateInputtedRightVelocity(leftRightInput);

        this.IsWalking = (velocityForward + velocityRight).magnitude > 0 ? true : false;

        this.rigidbody.velocity = velocityForward + velocityRight;
        Vector3 currentRigidbodyVelocity = this.rigidbody.velocity;
        currentRigidbodyVelocity.y = currentYVelocity;
        this.rigidbody.velocity = currentRigidbodyVelocity;
    }


    private Vector3 CalculateInputtedForwardVelocity(float forwardsBackInput)
    {
        return this.walkSpeed * forwardsBackInput * transform.forward;
    }
    private Vector3 CalculateInputtedRightVelocity(float leftRightInput)
    {
        return this.walkSpeed * leftRightInput * transform.right;
    }
}
}
