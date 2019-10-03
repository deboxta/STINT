using System;
using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    [RequireComponent(typeof(PlayerMover), typeof(PlayerJumpGravity))]
    public class Player : MonoBehaviour
    {
        private const int MAX_MENTAL_HEALTH = 100;
        
        private PlayerHitEventChannel playerHitEventChannel;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private PlayerMover mover;
        private int mentalHealth;

        private void Awake()
        {
            playerHitEventChannel = Finder.PlayerHitEventChannel;
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            mover = GetComponent<PlayerMover>();

            mentalHealth = MAX_MENTAL_HEALTH;
        }
        
        private void OnEnable()
        {
            playerHitEventChannel.OnPlayerHit += Hit;
        }

        private void OnDisable()
        {
            playerHitEventChannel.OnPlayerHit -= Hit;
        }

        private void Update()
        {
            if (mentalHealth <= 0)
            {
                Die();
            }
        }

        private void FixedUpdate()
        {
            var direction = Vector2.zero;
            if (Input.GetKey(KeyCode.D) ||
                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
                direction += Vector2.right;
            if (Input.GetKey(KeyCode.A) ||
               GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
                direction += Vector2.left;
            mover.Move(direction);
            
            if (Input.GetKeyDown(KeyCode.Space) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
            {
                mover.Jump();
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