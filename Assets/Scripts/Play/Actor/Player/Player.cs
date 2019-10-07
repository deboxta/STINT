using System;
using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    [Findable(R.S.Tag.Player)]
    [RequireComponent(typeof(PlayerMover), typeof(PlayerJumpGravity))]
    public class Player : MonoBehaviour
    {
        private const int MAX_MENTAL_HEALTH = 100;
        
        private PlayerHitEventChannel playerHitEventChannel;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private int mentalHealth;

        private void Awake()
        {
            playerHitEventChannel = Finder.PlayerHitEventChannel;
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;

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
        
        public void Hit()
        {
            Die();
        }

        public void Die()
        {
            playerDeathEventChannel.NotifyPlayerDeath();
        }
    }
}