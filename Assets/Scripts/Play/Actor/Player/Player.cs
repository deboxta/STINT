using System;
using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    [RequireComponent(typeof(PlayerMover), typeof(PlayerJumpGravity))]
    public class Player : MonoBehaviour
    {
        private const int MAX_MENTAL_HEALTH = 100;
        
        private PlayerHitEventChannel playerHitEventChannel;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private Hands hands;
        private Sensor sensor;
        private int mentalHealth;
        private bool holdingBox;

        private void Awake()
        {
            playerHitEventChannel = Finder.PlayerHitEventChannel;
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;

            hands = GetComponentInChildren<Hands>();
            sensor = GetComponentInChildren<Sensor>();
            
            mentalHealth = MAX_MENTAL_HEALTH;
        }
        
        private void OnEnable()
        {
            playerHitEventChannel.OnPlayerHit += Hit;
        }

        private void OnDisable()
        {
            playerHitEventChannel.OnPlayerHit -= Hit;
        }

        private void Update()
        {
            if (mentalHealth <= 0)
            {
                Die();
            }
        }
        
        public void Hit()
        {
            Die();
        }

        public void Die()
        {
            playerDeathEventChannel.NotifyPlayerDeath();
        }

        public void GrabBox()
        {
            var boxSensor = sensor.For<Box>();
            if (boxSensor.SensedObjects.Count > 0)
            {
                Box box = boxSensor.SensedObjects[0];

                box.transform.SetParent(hands.transform);
                box.GetRigidBody2D().isKinematic = true;
                box.GetRigidBody2D().simulated = false;
                if (box.transform.position.x < transform.position.x)
                {
                     box.transform.localPosition = new Vector3(-2, 0);
                }
                else
                {
                    box.transform.localPosition = new Vector3(2, 0);
                }
                
                holdingBox = true;
            }
        }
    }
}