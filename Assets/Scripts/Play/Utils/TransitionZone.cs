using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Game
{
    public class TransitionZone : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] private CompositeCollider2D roomOne;
        [SerializeField] private CompositeCollider2D roomTwo;
        [SerializeField] private bool moveHorizontaly  = true;

        private ISensor<Player> playerSensor;

        private void Awake()
        {
            playerSensor = GetComponent<Sensor>().For<Player>();
        }

        private void OnEnable()
        {
            playerSensor.OnSensedObject += OnPlayerSensed;
            playerSensor.OnUnsensedObject += OnPlayerUnSensed;
        }

        private void OnDisable()
        {
            playerSensor.OnSensedObject -= OnPlayerSensed;
            playerSensor.OnUnsensedObject -= OnPlayerUnSensed;
        }

        private void OnPlayerSensed(Player sensedPlayer)
        {
            var sensedPlayerRigidbody2D = sensedPlayer.GetComponent<Rigidbody2D>();
            sensedPlayerRigidbody2D.isKinematic = true;

            if (moveHorizontaly)
            {
                var playerVelocity = sensedPlayerRigidbody2D.velocity;
                
                playerVelocity.y = 0;
                sensedPlayerRigidbody2D.velocity = playerVelocity;
            }
            else
            {
                var playerVelocity = sensedPlayerRigidbody2D.velocity;
                
                playerVelocity.x = 0;
                sensedPlayerRigidbody2D.velocity = playerVelocity;
            }
            
            sensedPlayer.GetComponent<PlayerInput>().enabled = false;

            var cinemachineConfiner = cinemachineVirtualCamera.GetComponent<CinemachineConfiner>();
            cinemachineConfiner.m_BoundingShape2D =
                (cinemachineConfiner.m_BoundingShape2D == roomOne) ? roomTwo : roomOne;
            
            //Need to call this function when the confiner is change during runtime
            cinemachineConfiner.InvalidatePathCache();
        }

        private void OnPlayerUnSensed(Player sensedPlayer)
        {
            var sensedPlayerRigidbody2D = sensedPlayer.GetComponent<Rigidbody2D>();
            sensedPlayerRigidbody2D.isKinematic = false;
            sensedPlayer.GetComponent<PlayerInput>().enabled = true;
            
        }
    }
}