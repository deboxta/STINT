﻿using System;
using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    [Findable(R.S.Tag.Player)]
    
    [RequireComponent(typeof(PlayerMover), typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        private PlayerDeathEventChannel playerDeathEventChannel;
        private Sensor sensor;
        private ISensor<Box> boxSensor;
        private Hands hands;
        private bool isLookingRight;
        private Vitals vitals;
        
        public Hands Hands => hands;
        public Vitals Vitals
        {
            get => vitals;
        }
        public bool IsLookingRight 
        { 
            set => isLookingRight = value;
        }

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;

            hands = GetComponentInChildren<Hands>();
            sensor = GetComponentInChildren<Sensor>();
            vitals = GetComponent<Vitals>();
            
            isLookingRight = true;
            
            boxSensor = sensor.For<Box>();
        }

        private void FixedUpdate()
        {
            FlipPlayer();
        }

        //Turn the player in the right direction (and the box in his hand technicly)
        private void FlipPlayer()
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

        //TODO : LOOK FOR THE NEAREST BOX IN CASE THERE'S TWO
        //Grabs the box
        public void GrabBox()
        {
            //If the player isn't holding a box and if there is a box in his sensor
            if (!hands.IsHoldingBox && boxSensor.SensedObjects.Count > 0)
            {
                //Grabs the box
                hands.Grab(boxSensor.SensedObjects[0]);
            }
        }
        
        public void ThrowBox()
        {
            hands.Throw(isLookingRight);
        }
    }
}