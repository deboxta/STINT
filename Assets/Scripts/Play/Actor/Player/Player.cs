using System;
using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    [RequireComponent(typeof(PlayerMover))]
    public class Player : MonoBehaviour
    {
        private const int MAX_MENTAL_HEALTH = 100;
        
        private PlayerHitEventChannel playerHitEventChannel;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private Hands hands;
        private Sensor sensor;
        private int mentalHealth;
        private Box inHandBox;
        
        private bool isHoldingBox;
        public bool IsHoldingBox 
        {
            get => isHoldingBox;
            set => isHoldingBox = value;
        }
        
        private bool isLookingRight;
        public bool IsLookingRight 
        {
            set => isLookingRight = value;
        }


        private void Awake()
        {
            playerHitEventChannel = Finder.PlayerHitEventChannel;
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;

            hands = GetComponentInChildren<Hands>();
            sensor = GetComponentInChildren<Sensor>();
            
            mentalHealth = MAX_MENTAL_HEALTH;

            isLookingRight = true;
            isHoldingBox = false;
        }
        
        private void OnEnable()
        {
          //  playerHitEventChannel.OnPlayerHit += Hit;
        }

        private void OnDisable()
        {
            //playerHitEventChannel.OnPlayerHit -= Hit;
        }

        private void Update()
        {
            if (mentalHealth <= 0)
            {
                Die();
            }
        }

        private void FixedUpdate()
        {
            if (!isLookingRight)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
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
                inHandBox = boxSensor.SensedObjects[0];

                inHandBox.transform.SetParent(hands.transform);
                inHandBox.GetRigidBody2D().simulated = false;
                inHandBox.transform.localPosition = Vector3.zero;

                isHoldingBox = true;
            }
        }
        
        public void ThrowBox()
        {
            if (isHoldingBox)
            {
                  inHandBox.transform.parent = null;
                  inHandBox.GetRigidBody2D().simulated = true;

                  inHandBox.GetRigidBody2D().velocity = new Vector2(25, 30);
                  
                  isHoldingBox = false;
            }
          
        }
    }
}