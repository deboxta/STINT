using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class SuccessDetector : MonoBehaviour
    {
        private PlayerDeathEventChannel playerDeathEventChannel;
        private GameWonEventChannel gameWonEventChannel;
        private Dispatcher dispatcher;
        private bool firstDeath;
        private bool wonWithoutDying;

        private void Awake()
        {
            dispatcher = Finder.Dispatcher;
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            gameWonEventChannel = Finder.GameWonEventChannel;
            firstDeath = false;
            wonWithoutDying = false;

            CheckAlreadyUnlockedSuccess();
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
            if (dispatcher.DataCollector.NbDeath == 1)
            {
                firstDeath = true;
                //Show notification and save achievement
                playerDeathEventChannel.OnPlayerDeath -= OnPlayerDeath;
            }
        }

        private void OnGameWon()
        {
            if (dispatcher.DataCollector.NbDeath == 0)
            {
                wonWithoutDying = true;
                //Show notification and save achievement
                gameWonEventChannel.OnGameWon -= OnGameWon;
            }
        }

        private void CheckAlreadyUnlockedSuccess()
        {
            /*if (dispatcher.DataCollector.FirstDeath == true)
            {
                firstDeath = true;
            }

            if (expr)
            {
                
            }*/
        }
    }
}