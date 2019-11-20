using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Game
{
    public class TransitionZone : MonoBehaviour
    {
        //[SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        //[SerializeField] private CompositeCollider2D roomOne;
        //[SerializeField] private CompositeCollider2D roomTwo;
        [SerializeField] private bool moveHorizontal = true;
        //[SerializeField] private bool roomOneIsLeftOrBottom = true;
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

            //var cinemachineConfiner = cinemachineVirtualCamera.GetComponent<CinemachineConfiner>();
            
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
                    /*if (roomOneIsLeftOrBottom && cinemachineConfiner.m_BoundingShape2D == roomOne)
                    {
                        playerVelocity.x = minSpeed;
                    }
                    else
                    {
                        playerVelocity.x = -minSpeed;
                    }*/
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
                    
                    /*if (roomOneIsLeftOrBottom && cinemachineConfiner.m_BoundingShape2D == roomOne)
                    {
                        playerVelocity.y = minSpeed;
                    }
                    else
                    {
                        playerVelocity.y = -minSpeed;
                    }*/
                }
                
                sensedPlayerRigidbody2D.velocity = playerVelocity;
            }
            
            sensedPlayer.GetComponent<PlayerInput>().enabled = false;
            
            /*cinemachineConfiner.m_BoundingShape2D =
                (cinemachineConfiner.m_BoundingShape2D == roomOne) ? roomTwo : roomOne;
            
            //Need to call this function when the confiner is change during runtime
            cinemachineConfiner.InvalidatePathCache();*/
        }

        private void OnPlayerUnSensed(Player sensedPlayer)
        {
            var sensedPlayerRigidbody2D = sensedPlayer.GetComponent<Rigidbody2D>();
            sensedPlayerRigidbody2D.isKinematic = false;
            sensedPlayer.GetComponent<PlayerInput>().enabled = true;
            
        }
    }
}