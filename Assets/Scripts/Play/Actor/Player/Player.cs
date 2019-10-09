﻿using System;
using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    [RequireComponent(typeof(PlayerMover), typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMover), typeof(PlayerJumpGravity))]
    [Findable(R.S.Tag.Player)]

    public class Player : MonoBehaviour
    {
        private const int MAX_MENTAL_HEALTH = 100;
        
        private PlayerDeathEventChannel playerDeathEventChannel;
        private Hands hands;
        private Sensor sensor;
        private int mentalHealth;
        private bool holdingBox;

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;

            hands = GetComponentInChildren<Hands>();
            sensor = GetComponentInChildren<Sensor>();
            
            mentalHealth = MAX_MENTAL_HEALTH;
        }

        private void Update()
        {
            if (mentalHealth <= 0)
            {
                Die();
            }
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