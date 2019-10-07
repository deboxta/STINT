using System;
using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    [RequireComponent(typeof(PlayerMover),
        typeof(PlayerJumpGravity),
        typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        private const int MAX_MENTAL_HEALTH = 100;

        private PlayerHitEventChannel playerHitEventChannel;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private int mentalHealth;
        private Sensor Sensor { get; set; }
        public ISensor<SpikesTrap> SpikesTrapSensor { get; private set; }

        private void Awake()
        {
            playerHitEventChannel = Finder.PlayerHitEventChannel;
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            mentalHealth = MAX_MENTAL_HEALTH;
            Sensor = GetComponentInChildren<Sensor>();
            SpikesTrapSensor = Sensor.For<SpikesTrap>();
        }

        private void OnEnable()
        {
            playerHitEventChannel.OnPlayerHit += Hit;
            SpikesTrapSensor.OnSensedObject += OnSpikesTrapSensed;
        }

        private void OnDisable()
        {
            playerHitEventChannel.OnPlayerHit -= Hit;
            SpikesTrapSensor.OnSensedObject -= OnSpikesTrapSensed;
        }

        private void OnSpikesTrapSensed(SpikesTrap otherObject)
        {
            Debug.Log("SpikesTrap sensed");
            Die();
        }

        private void Update()
        {
            if (mentalHealth <= 0)
            {
                Die();
            }
        }

        private void Hit()
        {
            Die();
        }

        private void Die()
        {
            playerDeathEventChannel.NotifyPlayerDeath();
        }
    }
}