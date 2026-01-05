using System;
using System.Collections.Generic;
using UnityEngine;

namespace KartGame.Track
{
    /// <summary>
    /// A class to display information about a particular racer's timings.  WARNING: This class uses strings and creates a small amount of garbage each frame.
    /// </summary>
    public class TimeDisplay : MonoBehaviour
    {
        public static Action OnUpdateLap;
        public static Action<int> OnSetLaps;
        
        private List<float> finishedLapTimes = new List<float>();

        private float currentLapStartTime;


        private bool lapsOver;

        void OnEnable()
        {
            OnUpdateLap += UpdateLap;
        }

        void OnDisable()
        {
            OnUpdateLap -= UpdateLap;
        }

        int getBestLap()
        {
            int best = -1;
            for (int i = 0; i < finishedLapTimes.Count; i++)
            {
                if (best < 0 || finishedLapTimes[i] < finishedLapTimes[best]) best = i;
            }

            return best;
        }

        void UpdateLap()
        {
            if (lapsOver) return;

            if (currentLapStartTime == 0)
            {
                currentLapStartTime = Time.time;
                return;
            }

            finishedLapTimes.Add(Time.time - currentLapStartTime);
            currentLapStartTime = Time.time;
        }

        void Update()
        {
            if (currentLapStartTime == 0) return;
            if (lapsOver) return;
        }

        string DisplayCurrentLapTime()
        {
            float currentLapTime = Time.time - currentLapStartTime;
            if (currentLapTime < 0.01f) return "0:00";
            return getTimeString(currentLapTime);
        }
    
        string getTimeString(float time){
            int timeInt = (int)(time);
            int minutes = timeInt / 60;
            int seconds = timeInt % 60;
            float fraction = (time * 100) % 100;
            if (fraction > 99) fraction = 99;
            return string.Format("{0}:{1:00}:{2:00}", minutes, seconds, fraction);
        }

        string DisplaySessionBestLapTime()
        {
            int bestLap = getBestLap();
            if (bestLap < 0) return "";
            return getTimeString(finishedLapTimes[bestLap]);
        }
    }
}
