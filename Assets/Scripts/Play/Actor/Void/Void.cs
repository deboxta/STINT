﻿using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class Void : MonoBehaviour
    {
        private ISensor<Player> playerSensor;

        private void Awake()
        {
            playerSensor = GetComponent<Sensor>().For<Player>();
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