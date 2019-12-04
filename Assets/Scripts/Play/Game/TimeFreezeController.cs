using System;
using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    [Findable(R.S.Tag.GameController)]
    public class TimeFreezeController : MonoBehaviour
    {
        [SerializeField] private bool startFrozen = false;
        [SerializeField] private bool changeStateAtInterval = false;
        [SerializeField] private float changeStateDelay = 5;
        [SerializeField] private float initialChangeStateDelay = 2;
        [SerializeField] private float warningDuration = 1f;
        
        private float timeLeftBeforeWarning;
        private float timeLeftUntilTimeFreezeChange;
        
        private bool isFrozen;
        private bool isWarning;
        private TimeFreezeEventChannel timeFreezeEventChannel;
        private TimeFreezeWarningEventChannel timeFreezeWarningEventChannel;

        public bool IsFrozen
        {
            get => isFrozen;
            private set
            {
                isFrozen = value;
#if UNITY_EDITOR
                Debug.Log("Time frozen: " + value);
#endif
                timeFreezeEventChannel.NotifyTimeFreezeStateChanged();
            }
        }

        private void Awake()
        {
            timeFreezeEventChannel = Finder.TimeFreezeEventChannel;
            timeFreezeWarningEventChannel = Finder.TimeFreezeWarningEventChannel;
            timeLeftBeforeWarning = initialChangeStateDelay;
            timeLeftUntilTimeFreezeChange = warningDuration;
        }

        private void Start()
        {
            isFrozen = startFrozen;
            isWarning = false;
        }

        public void Reset()
        {
            isFrozen = false;
        }

        public void SwitchState()
        {
            IsFrozen = !IsFrozen;
            isWarning = false;
            timeLeftBeforeWarning = changeStateDelay;
            timeLeftUntilTimeFreezeChange = warningDuration;
        }

        // Author : Sébastien Arsenault
        private void Update()
        {
            if (changeStateAtInterval)
            {
                if (timeLeftBeforeWarning >= 0f)
                {
                    timeLeftBeforeWarning -= Time.deltaTime;
                }
                else
                {
                    if (!isWarning)
                    {
                        timeFreezeWarningEventChannel.NotifyTimeFreezeWarning();
                        isWarning = true;
                    }

                    if (timeLeftUntilTimeFreezeChange >= 0f)
                    {
                        timeLeftUntilTimeFreezeChange -= Time.deltaTime;
                    }
                    else
                    {
                        SwitchState();
                    }
                }
            }
        }
    }
}