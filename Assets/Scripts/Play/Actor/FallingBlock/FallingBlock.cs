using System;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class FallingBlock : MonoBehaviour
    {
        [SerializeField] private float fallSpeed = 5f;
        
        private Rigidbody2D rigidBody2D;
        private DeadlyTrap deadlyTrap;
        private ISensor<Player> playerSensor;
        
        private void Awake()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();
            playerSensor = GetComponentInChildren<Sensor>().For<Player>();
            deadlyTrap = GetComponentInChildren<DeadlyTrap>();
        }

        private void OnEnable()
        {
            playerSensor.OnSensedObject += OnPlayerSensed;
        }

        private void OnDisable()
        {
            playerSensor.OnSensedObject -= OnPlayerSensed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var otherParentTransform = other.transform.parent;
            
            if (!IsSelf(otherParentTransform))
            {
                StopFalling();
            }
        }
        
        private bool IsSelf(Transform otherParentTransform)
        {
            return transform == otherParentTransform;
        }

        private void Fall()
        {
            rigidBody2D.isKinematic = false;
        }

        private void StopFalling()
        {
            rigidBody2D.velocity = Vector2.zero;
            rigidBody2D.isKinematic = true;

            deadlyTrap.enabled = false;
        }
        
        private void OnPlayerSensed(Player player)
        {
            Fall();
        }
    }
}