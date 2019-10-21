using System;
using UnityEngine;

namespace Game
{
    public class FallingBlock : MonoBehaviour
    {
        [SerializeField] private float fallSpeed = 5f;
        
        private Rigidbody2D rigidBody2D;
        private BoxCollider2D boxCollider2D;
        private ISensor<Player> playerSensor;
        
        private void Awake()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();
            boxCollider2D = GetComponentInChildren<BoxCollider2D>();
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

        private void FixedUpdate()
        {
            
        }

        private void Fall()
        {
            //rigidBody2D.velocity = Vector2.down * fallSpeed;
            rigidBody2D.isKinematic = false;
        }

        private void StopFalling()
        {
            rigidBody2D.velocity = Vector2.zero;
        }
        
        private void OnPlayerSensed(Player player)
        {
            Fall();
        }
    }
}