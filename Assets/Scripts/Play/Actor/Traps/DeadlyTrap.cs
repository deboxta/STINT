using System;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    [Obsolete("The old sensor is deprecated. Use " + nameof(DeadlyTrapV2) + " instead.")]
    public class DeadlyTrap : MonoBehaviour
    {
        private ISensor<Player> playerSensor;

        private void Awake()
        {
            playerSensor = GetComponentInChildren<Sensor>().For<Player>();
        }

        private void OnEnable()
        {
            playerSensor.OnSensedObject += OnPlayerSensed;
        }

        private void OnDisable()
        {
            playerSensor.OnSensedObject -= OnPlayerSensed;
        }

        private void OnPlayerSensed(Player otherObject)
        {
            otherObject.Die();
        }
    }
}