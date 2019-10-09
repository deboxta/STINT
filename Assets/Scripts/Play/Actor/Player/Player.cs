using System;
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
        private Hands hands;
        private Sensor sensor;
        private bool holdingBox;
        private Vitals vitals;

        public Vitals Vitals
        {
            get => vitals;
        }

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;

            hands = GetComponentInChildren<Hands>();
            sensor = GetComponentInChildren<Sensor>();
            vitals = GetComponent<Vitals>();
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