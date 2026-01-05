using System;
using UnityEngine;
using System.Collections.Generic;

namespace UFO_PlayerStuff
{
    public class PlayerAudio : MonoBehaviour
    {
        public enum PlayOverideModes
        {
            NO_OVERIDE,
            OVERRIDE
        };

        [SerializeField] 
        protected List<AudioClip> audioClips = new List<AudioClip>();

        protected Dictionary<string, AudioClip> clipNames = new Dictionary<string, AudioClip>();

        public void Awake()
        {

            foreach (AudioClip clip in audioClips)
            {
                clipNames.Add(clip.name, clip);
            }
        }

        /// <summary>
        /// This function will play a sound at the given index of its audioClips list
        /// it will throw an argument out of range exception if the index given is negative
        /// or the same size as or bigger than the list of audio clips on the player audio obeject
        /// </summary>
        /// <param name="index">The index of the sound to be played</param>
        public void PlaySound(int index, PlayOverideModes overideMode = PlayOverideModes.NO_OVERIDE)
        {
            bool invalidIndex = index < 0 || index >= this.audioClips.Count;
            if (invalidIndex)
                Debug.LogError("Sound requested invalid. " +
                               "This may be because the index given is negative " +
                               "or out of bounds of the list of sounds on the player audio object");
            
        }
        

        public void PlaySound(string soundName, PlayOverideModes overideMode = PlayOverideModes.NO_OVERIDE)
        {
            try
            {
            }
            catch (KeyNotFoundException)
            {
                Debug.LogError("Sound requested invalid. " +
                "This is because the name given was not found in the player audio object");
            }
        }
    }
}
