using System;
using System.Collections;
using System.Diagnostics;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public class ThrowableObject : MonoBehaviour, IFreezable
    {
        private TimeFreezeEventChannel timeFreezeEventChannel;
        private Vector2 velocityBeforeFreeze;
        private ObjectThrower thrower;
        private Stopwatch removeObjectStopwatch;
        private TimeSpan removeObjectDelay;

        public bool IsFrozen => Finder.TimeFreezeController.IsFrozen;
        public Rigidbody2D Rigidbody2D { get; set; }
        private void Awake()
        {
            timeFreezeEventChannel = Finder.TimeFreezeEventChannel;
            Rigidbody2D = GetComponent<Rigidbody2D>();
            removeObjectStopwatch = new Stopwatch();
            thrower = GetComponentInParent<ObjectThrower>();
        }

        private void OnEnable()
        {
            timeFreezeEventChannel.OnTimeFreezeStateChanged += OnTimeFreezeStateChanged;
            removeObjectDelay = TimeSpan.FromSeconds(thrower.removeObjectDelay);
            removeObjectStopwatch.Restart();
        }

        private void OnDisable()
        {
            timeFreezeEventChannel.OnTimeFreezeStateChanged -= OnTimeFreezeStateChanged;
        }

        private void FixedUpdate()
        {
            if (removeObjectStopwatch.Elapsed >= removeObjectDelay)
            {
                removeObjectStopwatch.Stop();
                thrower.RemoveThrownObject(this);
            }
            
            if (!IsFrozen)
                Rigidbody2D.velocity += Physics2D.gravity * Time.fixedDeltaTime;
        }
        
        private void OnTimeFreezeStateChanged()
        {
            if (IsFrozen)
            {
                removeObjectStopwatch.Stop();
                velocityBeforeFreeze = Rigidbody2D.velocity;
                Rigidbody2D.velocity = Vector2.zero;
            }
            else
            {
                removeObjectStopwatch.Start();
                Rigidbody2D.velocity = velocityBeforeFreeze;
                velocityBeforeFreeze = Vector2.zero;
            }
        }
    }
}