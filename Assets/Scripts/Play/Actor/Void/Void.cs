using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class Void : MonoBehaviour
    {
        private PlayerHitEventChannel playerHitEventChannel;
        private Sensor sensor;

        private void Awake()
        {
            playerHitEventChannel = Finder.PlayerHitEventChannel;
            sensor = GetComponent<Sensor>();
        }

        private void Update()
        {
            var playerSensor = sensor.For<Player>();

            if (playerSensor != null && playerSensor.SensedObjects.Count >= 1)
            {
                playerSensor.SensedObjects[0].Die();
            }
        }
    }
}