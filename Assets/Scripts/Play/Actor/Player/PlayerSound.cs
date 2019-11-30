using System;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class PlayerSound : MonoBehaviour
    {
        [SerializeField] private AudioClip deathAudioClip;
        [SerializeField] private AudioClip jumpAudioClip;
        
        private AudioSource audioSource;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private PlayerMover playerMover;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            playerMover = GetComponentInParent<PlayerMover>();
        }

        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += OnPlayerDeath;
            playerMover.OnPlayerJump += OnPlayerJump;

        }

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= OnPlayerDeath;
            playerMover.OnPlayerJump -= OnPlayerJump;
        }

        private void OnPlayerDeath()
        {
            audioSource.clip = deathAudioClip;
            audioSource.Play();
        }

        private void OnPlayerJump()
        {
            audioSource.clip = jumpAudioClip;
            audioSource.Play();
        }
    }
}