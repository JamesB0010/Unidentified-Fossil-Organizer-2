using System;
using System.Collections;
using System.Collections.Generic;
using UFO_UI;
using UnityEngine;
using UnityEngine.Events;

namespace UFO_PickupStuff
{
//this is just a class which groups data like a struct
    [System.Serializable]
    class HoldObjectPhysicsData
    {
        public float throwScalar = 7f, upThrowAddVec = 2f, dampingFactorScalar = 5f, speedScalar = 6.13f;
        public GameObject objectInRange;

        public HoldObjectPhysicsData(float throwScalar, float upThrowAddVec, float dampingFactorScalar,
            float speedScalar)
        {
            this.throwScalar = throwScalar;
            this.upThrowAddVec = upThrowAddVec;
            this.dampingFactorScalar = dampingFactorScalar;
            this.speedScalar = speedScalar;
        }
    }

//again this is just a class which groups data like a struct
    class AbleToPickupStateData
    {
        public AbleToPickupStateData(bool readyToPickup = false, bool interactButtonActive = false,
            bool holdingObject = false, float droppedObjectTimeStamp = 0f)
        {
            this.readyToPickup = readyToPickup;
            this.interactButtonActive = interactButtonActive;
            this.holdingObject = holdingObject;
            this.droppedObjectTimeStamp = droppedObjectTimeStamp;
        }

        public bool readyToPickup = false;
        public bool interactButtonActive = false;
        public bool holdingObject = false;
        public float droppedObjectTimeStamp = 0;
    }

//The meat of the script
    [RequireComponent(typeof(UFO_PlayerStuff.CameraForwardsSampler))]
    public class PickupScript : MonoBehaviour
    {
        #region Attributes

        protected UFO_PlayerStuff.CameraForwardsSampler spaceSampler;

        //this has to be set in the editor
        [SerializeField] private Transform holdItemLocation;

        private HoldObjectPhysicsData holdPhysicsData = new HoldObjectPhysicsData(7f, 2f, 5f, 6.13f);

        private AbleToPickupStateData pickupStateData = new AbleToPickupStateData();

        public bool IsHolding()
        {
            return this.pickupStateData.holdingObject;
        }

        [SerializeField] protected UFO_PlayerStuff.PlayerAudio playerAudio = null;

        //some events which will be broadcasted/Invoked and can be reacted to by other game objects
        [SerializeField] private UnityEvent pickedUpObject = new UnityEvent();

        [SerializeField] private UnityEvent droppedObject = new UnityEvent();

        private float droppedObjectTimestamp;

        public float DroppedObjectTimestamp
        {
            get => droppedObjectTimestamp;

            set
            {
                this.droppedObjectTimestamp = value;
            }
        }
        #endregion

        //please read these methods in a top down order as the order of definitions
        //is the same or similar as the order the methods are called
        protected void Start()
        {
            SetSpaceSampler();
            if (playerAudio == null)
                playerAudio = gameObject.AddComponent<UFO_PlayerStuff.PlayerAudio>();
        }

        protected void SetSpaceSampler()
        {
            this.spaceSampler = gameObject.GetComponent<UFO_PlayerStuff.CameraForwardsSampler>();
        }

        private void Update()
        {
            //Debug.Log(this.pickupStateData.holdingObject);
            int pickupObject = (int)Input.GetAxis("Pickup Object");
            if (pickupObject == 1)
            {
                this.InteractButtonActive = true;
            }
            else
            {
                this.InteractButtonActive = false;
            }
        }

        private bool InteractButtonActive
        {
            set
            {
                //To know when we have lifted our finger off the key
                //we have to know the state of the Interact button last frame
                if (InteractKeyPreviousStateUp())
                {
                    this.pickupStateData.interactButtonActive = value;
                    return;
                }

                //if the interact button is still being held then quit
                //we only want the interaction when the button is lifted up
                if (value == true)
                {
                    //finally set interactButtonActive
                    this.pickupStateData.interactButtonActive = value;
                    return;
                }

                //if your holding an object drop it
                if (this.pickupStateData.holdingObject)
                {
                    DropObject();
                    return;
                }

                //otherwise pick up an object
                bool canPickupObject = this.pickupStateData.readyToPickup == true && HasMinimumTimePassedSinceObjDrop();
                if (canPickupObject)
                {
                    this.PickupObject();
                }

                //finally set interactButtonActive
                this.pickupStateData.interactButtonActive = value;
            }
        }


        private bool InteractKeyPreviousStateUp()
        {
            return this.pickupStateData.interactButtonActive == false;
        }

        private bool HasMinimumTimePassedSinceObjDrop()
        {
            return Time.time - this.pickupStateData.droppedObjectTimeStamp > 0.1;
        }

        private void DropObject()
        {
            this.droppedObjectTimestamp = Time.time;
            this.pickupStateData.readyToPickup = true;
            this.pickupStateData.holdingObject = false;

            //we record a timestamp so the object weve just dropped isnt picked back up at the end of the frame
            //Or in the next frame when the input for dropping it is still on
            //if we didnt record this whenever we would drop an object it would be picked back up the next frame!
            this.pickupStateData.droppedObjectTimeStamp = Time.time;
            PushHeldObjectAway();
            this.droppedObject.Invoke();
            this.PlayThrowSound();
            
            this.spaceSampler.PickupableObjectOutOfRangeUnityEvent?.Invoke();
        }

        private void PushHeldObjectAway()
        {
            //get the direction we will throw the object
            Vector3 directionToThrow = gameObject.transform.forward.normalized;

            //Multiply the direction by some scalar to get the velocity of the throw
            Vector3 velocityOfThrow = directionToThrow * this.holdPhysicsData.throwScalar;

            //offset the velocity of the throw to nudge its throw
            velocityOfThrow += new Vector3(0, this.holdPhysicsData.upThrowAddVec, 0);

            //finally apply the impulse
            this.holdPhysicsData.objectInRange.GetComponent<Rigidbody>().AddForce(velocityOfThrow, ForceMode.Impulse);
        }

        private void PickupObject()
        {
            this.pickupStateData.holdingObject = true;
            this.pickupStateData.readyToPickup = false;
            this.pickedUpObject.Invoke();

            PlayPickupSound();
        }

        private void PlayPickupSound()
        {
            this.playerAudio.PlaySound("itemPickup");
        }

        private void PlayThrowSound()
        {
            this.playerAudio.PlaySound("ThrowSoundEffect");
        }

        private void FixedUpdate()
        {
            if (this.pickupStateData.holdingObject)
            {
                MoveObjectToHoldPoint();
            }
        }

        private void MoveObjectToHoldPoint()
        {
            //part of this function has been generated by chat gpt
            Rigidbody objectRb = this.holdPhysicsData.objectInRange.GetComponent<Rigidbody>();

            Vector3 forceVector = this.holdItemLocation.position - objectRb.position;

            //Calculate the distance to the target position
            float distanceToTarget = forceVector.magnitude;

            //apply damping factor based on distance
            float dampingFactor = Mathf.Clamp01(distanceToTarget / this.holdPhysicsData.dampingFactorScalar);

            //calculate desired velocity
            Vector3 desiredVelocity = forceVector.normalized * this.holdPhysicsData.speedScalar;

            //calculate the change in velocity needed
            Vector3 deltaVelocity = (desiredVelocity - objectRb.velocity) * dampingFactor;

            //calculate the force needed based on change in velocity
            Vector3 force = deltaVelocity / Time.fixedDeltaTime;

            objectRb.AddForce(force, ForceMode.Force);
        }


        //this function reacts to the CameraForwardsSampler finding a pickupable object in range
        //it is set in the editor if you go to the player camera under the CamForSampler script
        public void UpdateItemToBePickedUp()
        {
            bool alreadyHoldingAnObject = this.pickupStateData.holdingObject;
            if (alreadyHoldingAnObject)
                return;

            GetReadyToPickUp();
        }

        private void GetReadyToPickUp()
        {
            this.pickupStateData.readyToPickup = true;
            this.holdPhysicsData.objectInRange = this.spaceSampler.ObjectInRange;
        }

        //this function reacts to the CameraForwardsSampler finding a pickupable object in range
        //it is set in the editor if you go to the player camera under the CamForSampler script
        public void ObjectToBePickedUpOutOfRange()
        {
            this.pickupStateData.readyToPickup = false;
        }


    }
}