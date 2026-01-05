using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// This class produces audio for various states of the vehicle's movement.
    /// </summary>
    public class Engine : MonoBehaviour
    {
        public float minRPM = 750;
        public float maxRPM = 5000;
        ArcadeKart arcadeKart;
        FMODUnity.StudioEventEmitter emitter;

        void Awake()
        {
            arcadeKart = GetComponentInParent<ArcadeKart>();
            this.emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        }

        void Update()
        {
            float kartSpeed = arcadeKart != null ? arcadeKart.LocalSpeed() : 0;
            
            // Ensure kartSpeed is always positive by using Mathf.Abs to get the absolute value
            float effectiveRPM = Mathf.Lerp(minRPM, maxRPM, Mathf.Abs(kartSpeed));

            // Set the RPM parameter for the FMOD event
            emitter.SetParameter("RPM", effectiveRPM);
        }
    }
}
