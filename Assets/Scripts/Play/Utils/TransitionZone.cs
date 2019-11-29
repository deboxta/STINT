using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class TransitionZone : MonoBehaviour
    {
        [SerializeField] private bool moveHorizontal = true;
        [SerializeField] [Range(1, 100)] private float minSpeed = 5f;

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

            if (moveHorizontal)
            {
                var playerVelocity = sensedPlayerRigidbody2D.velocity;
                
                playerVelocity.y = 0;
                //If the player is too slow, we speed up his transition
                if (Math.Abs(playerVelocity.x) < minSpeed)
                {
                    if (playerVelocity.x < 0)
                    {
                        playerVelocity.x = minSpeed;
                    }
                    else
                    {
                        playerVelocity.x = -minSpeed;
                    }
                }
                
                sensedPlayerRigidbody2D.velocity = playerVelocity;
            }
            else
            {
                var playerVelocity = sensedPlayerRigidbody2D.velocity;
                
                playerVelocity.x = 0;
                //If the player is too slow, we speed up his transition
                if (Math.Abs(playerVelocity.y) < minSpeed)
                {
                    if (playerVelocity.y < 0)
                    {
                        playerVelocity.y = minSpeed;
                    }
                    else
                    {
                        playerVelocity.y = -minSpeed;
                    }
                }
                
                sensedPlayerRigidbody2D.velocity = playerVelocity;
            }
            
            sensedPlayer.GetComponent<PlayerInput>().enabled = false;
        }

        private void OnPlayerUnSensed(Player sensedPlayer)
        {
            var sensedPlayerRigidbody2D = sensedPlayer.GetComponent<Rigidbody2D>();
            sensedPlayerRigidbody2D.isKinematic = false;
            sensedPlayer.GetComponent<PlayerInput>().enabled = true;
            
        }
    }
}