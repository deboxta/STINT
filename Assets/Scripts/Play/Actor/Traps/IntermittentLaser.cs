using System;
using System.Collections;
using System.Diagnostics;
using System.Timers;
using Harmony;
using UnityEngine;

namespace Game
{
    public class IntermittentLaser : ConstantLaser, IFreezable
    {
        [SerializeField] [Range(0, 100)] private float onTimeInSeconds = 1;
        [SerializeField] [Range(0, 100)] private float offTimeInSeconds = 1;
        
        private bool firing;
        private Stopwatch switchFiringStateStopwatch;
        private TimeSpan firingStopwatchCurrentTimeLimit;
        private TimeFreezeEventChannel timeFreezeEventChannel;

        private bool Firing
        {
            get => firing;
            set
            {
                firing = value;
                if (!value)
                    laserBeam.SetPosition(1, transform.position);
                laserBeam.gameObject.SetActive(value);
            }
        }

        public bool Frozen => Finder.TimeFreezeController.IsFrozen;

        protected override void Awake()
        {
            base.Awake();

            firing = true;
            switchFiringStateStopwatch = new Stopwatch();
            timeFreezeEventChannel = Finder.TimeFreezeEventChannel;
        }

        private void Start()
        {
            switchFiringStateStopwatch.Start();
        }

        private void OnEnable()
        {
            timeFreezeEventChannel.OnTimeFreezeStateChanged += OnTimeFreezeStateChanged;
        }

        private void OnDisable()
        {
            timeFreezeEventChannel.OnTimeFreezeStateChanged -= OnTimeFreezeStateChanged;
        }
        
        private void OnTimeFreezeStateChanged()
        {
            if (Frozen)
                switchFiringStateStopwatch.Stop();
            else
                switchFiringStateStopwatch.Start();
        }

        protected override void FixedUpdate()
        {
            if (Firing) 
                base.FixedUpdate();

            if (switchFiringStateStopwatch.Elapsed >= firingStopwatchCurrentTimeLimit)
            {
                Firing = !Firing;
                firingStopwatchCurrentTimeLimit = TimeSpan.FromSeconds(Firing ? onTimeInSeconds : offTimeInSeconds);
                switchFiringStateStopwatch.Restart();
            }
        }
    }
}