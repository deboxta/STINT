﻿using System;
using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    //Author : Sébastien Arsenault
    public class SuccessDetector : MonoBehaviour
    {
        private Dispatcher dispatcher;
        
        private FirstDeathSuccess firstDeathSuccess;
        private WonGameSuccess wonGameSuccess;
        private WonWithoutDyingSuccess wonWithoutDyingSuccess;
        private SaveNamedBenSuccess saveNamedBenSuccess;
        private SecretRoomFoundSuccess secretRoomFoundSuccess;

        private SuccessUnlockedEventChannel successUnlockedEventChannel;
        private SavedDataLoadedEventChannel savedDataLoadedEventChannel;
        private NewGameLoadedEventChannel newGameLoadedEventChannel;

        private void Awake()
        {
            dispatcher = Finder.Dispatcher;
            successUnlockedEventChannel = Finder.SuccessUnlockedEventChannel;
            savedDataLoadedEventChannel = Finder.SavedDataLoadedEventChannel;
            newGameLoadedEventChannel = Finder.NewGameLoadedEventChannel;
        }

        private void OnEnable()
        {
            newGameLoadedEventChannel.OnNewGameLoaded += OnNewGameLoaded;
            savedDataLoadedEventChannel.OnSavedDataLoaded += OnSavedDataLoaded;
        }

        private void OnDisable()
        {
            if (firstDeathSuccess != null)
            {
                firstDeathSuccess.OnFirstDeath -= OnFirstDeathDetected;
            }
            if (wonWithoutDyingSuccess != null)
            {
                wonWithoutDyingSuccess.OnGameWonWithoutDyingSuccess -= OnGameWonWithoutDyingWithoutDyingDetected;
            }
            if (wonGameSuccess != null)
            {
                wonGameSuccess.OnGameWonSuccess -= OnGameWonDetected;
            }
            if (saveNamedBenSuccess != null)
            {
                saveNamedBenSuccess.OnSaveNamedBen -= OnSaveNamedBenDetected;
            }
            if (secretRoomFoundSuccess != null)
            {
                secretRoomFoundSuccess.OnSecretRoomFound -= OnSecretRoomFoundDetected;
            }
            
            savedDataLoadedEventChannel.OnSavedDataLoaded -= OnSavedDataLoaded;
            
            newGameLoadedEventChannel.OnNewGameLoaded -= OnNewGameLoaded;
        }

        private void OnSuccessUnlocked(string successName)
        {
            successUnlockedEventChannel.NotifySuccessUnlocked(successName);
        }

        private void OnNewGameLoaded()
        {
            CheckAlreadyUnlockedSuccess();
        }

        private void OnSavedDataLoaded()
        {
            CheckAlreadyUnlockedSuccess();
        }

        private void OnSecretRoomFoundDetected()
        {
            dispatcher.DataCollector.SecretRoomFound = true;
            secretRoomFoundSuccess.OnSecretRoomFound -= OnSecretRoomFoundDetected;
            secretRoomFoundSuccess.DestroySuccess();
            OnSuccessUnlocked(secretRoomFoundSuccess.successName);
        }

        private void OnFirstDeathDetected()
        {
            dispatcher.DataCollector.FirstDeath = true;
            firstDeathSuccess.OnFirstDeath -= OnFirstDeathDetected;
            firstDeathSuccess.DestroySuccess();
            OnSuccessUnlocked(firstDeathSuccess.successName);
        }

        private void OnGameWonDetected()
        {
            dispatcher.DataCollector.WonGame = true;
            wonGameSuccess.OnGameWonSuccess -= OnGameWonDetected;
            wonGameSuccess.DestroySuccess();
            OnSuccessUnlocked(wonGameSuccess.successName);
        }
        
        private void OnGameWonWithoutDyingWithoutDyingDetected()
        {
            if (dispatcher.DataCollector.NbDeath == 0)
            {
                dispatcher.DataCollector.WonWithoutDying = true;
                wonWithoutDyingSuccess.OnGameWonWithoutDyingSuccess -= OnGameWonWithoutDyingWithoutDyingDetected;
                wonWithoutDyingSuccess.DestroySuccess();
                OnSuccessUnlocked(wonWithoutDyingSuccess.successName);
            }
        }

        private void OnSaveNamedBenDetected()
        {
            if (dispatcher.DataCollector.Name == "ben")
            {
                dispatcher.DataCollector.SaveNamedBen = true;
                saveNamedBenSuccess.OnSaveNamedBen -= OnSaveNamedBenDetected;
                saveNamedBenSuccess.DestroySuccess();
                OnSuccessUnlocked(saveNamedBenSuccess.successName);
            }
        }

        private void CheckAlreadyUnlockedSuccess()
        {
            if (!dispatcher.DataCollector.FirstDeath)
            {
                firstDeathSuccess = gameObject.AddComponent<FirstDeathSuccess>();
                firstDeathSuccess.OnFirstDeath += OnFirstDeathDetected;
            }

            if (!dispatcher.DataCollector.WonGame)
            {
                wonGameSuccess = gameObject.AddComponent<WonGameSuccess>();
                wonGameSuccess.OnGameWonSuccess += OnGameWonDetected;
            }
            
            if (!dispatcher.DataCollector.WonWithoutDying)
            {
                wonWithoutDyingSuccess = gameObject.AddComponent<WonWithoutDyingSuccess>();
                wonWithoutDyingSuccess.OnGameWonWithoutDyingSuccess += OnGameWonWithoutDyingWithoutDyingDetected;
            }

            if (!dispatcher.DataCollector.SaveNamedBen)
            {
                saveNamedBenSuccess = gameObject.AddComponent<SaveNamedBenSuccess>();
                saveNamedBenSuccess.OnSaveNamedBen += OnSaveNamedBenDetected;
            }

            if (!dispatcher.DataCollector.SecretRoomFound)
            {
                secretRoomFoundSuccess = gameObject.AddComponent<SecretRoomFoundSuccess>();
                secretRoomFoundSuccess.OnSecretRoomFound += OnSecretRoomFoundDetected;
            }
        }
    }
}