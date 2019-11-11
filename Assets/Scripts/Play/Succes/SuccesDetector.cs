﻿using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class SuccesDetector : MonoBehaviour
    {
        private PlayerDeathEventChannel playerDeathEventChannel;
        private GameWonEventChannel gameWonEventChannel;
        //private Serialize serialize;
        private bool first

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            gameWonEventChannel = Finder.GameWonEventChannel;

            CheckAlreadyUnlockedSucces();
        }

        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += OnPlayerDeath;
            gameWonEventChannel.OnGameWon += OnGameWon;
        }

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= OnPlayerDeath;
            gameWonEventChannel.OnGameWon -= OnGameWon;
        }

        private void OnPlayerDeath()
        {
            if (serialize.neverDied)
            {
                throw new NotImplementedException();
            }
        }

        private void OnGameWon()
        {
            if (expr)
            {
                throw new NotImplementedException();
            }
        }

        private void CheckAlreadyUnlockedSucces()
        {
            /*if (serialize.gameWon && serialize.neverDied)
            {
                gameWonEventChannel.OnGameWon -= OnGameWon;
            }

            if (expr)
            {
                
            }*/
        }
    }
}