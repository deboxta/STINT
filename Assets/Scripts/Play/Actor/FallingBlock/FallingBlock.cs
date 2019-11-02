using System;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class FallingBlock : MonoBehaviour, IFreezable
    {
        [SerializeField] private float fallSpeed = 5f;
        
        private new Rigidbody2D rigidbody2D;
        private DeadlyTrap deadlyTrap;
        private ISensor<Player> playerSensor;
        private TimeFreezeEventChannel timeFreezeEventChannel;
        private Vector2 velocityBeforeFreeze;
        private bool wasKinematicBeforeFreeze;

        public bool Frozen => Finder.TimeFreezeController.IsFrozen;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            playerSensor = GetComponentInChildren<Sensor>().For<Player>();
            deadlyTrap = GetComponentInChildren<DeadlyTrap>();
            timeFreezeEventChannel = Finder.TimeFreezeEventChannel;
            velocityBeforeFreeze = Vector2.zero;
            wasKinematicBeforeFreeze = true;
        }

        private void OnEnable()
        {
            timeFreezeEventChannel.OnTimeFreezeStateChanged += OnTimeFreezeStateChanged;
            playerSensor.OnSensedObject += OnPlayerSensed;
        }

        private void OnDisable()
        {
            timeFreezeEventChannel.OnTimeFreezeStateChanged -= OnTimeFreezeStateChanged;
            playerSensor.OnSensedObject -= OnPlayerSensed;
        }

        private void OnTimeFreezeStateChanged()
        {
            if (Frozen)
            {
                velocityBeforeFreeze = rigidbody2D.velocity;
                wasKinematicBeforeFreeze = rigidbody2D.isKinematic;
                StopFalling();
            }
            else
            {
                rigidbody2D.velocity = velocityBeforeFreeze;
                rigidbody2D.isKinematic = wasKinematicBeforeFreeze;
                if (!rigidbody2D.isKinematic)
                    deadlyTrap.enabled = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var otherParentTransform = other.transform.parent;
            
            if (!IsSelf(otherParentTransform))
            {
                StopFalling();
            }
        }
        
        private bool IsSelf(Transform otherParentTransform)
        {
            return transform == otherParentTransform;
        }

        private void Fall()
        {
            rigidbody2D.isKinematic = false;
        }

        private void StopFalling()
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.isKinematic = true;
            
            deadlyTrap.enabled = false;
        }
        
        private void OnPlayerSensed(Player player)
        {
            if (!Frozen)
                Fall();
        }
    }
}