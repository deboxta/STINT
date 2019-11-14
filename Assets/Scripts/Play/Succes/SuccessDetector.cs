﻿using System;
using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    public class SuccessDetector : MonoBehaviour
    {
        private Dispatcher dispatcher;
        private FirstDeathSuccess firstDeathSuccess;
        private WonGameSuccess wonGameSuccess;
        private WonWithoutDyingSuccess wonWithoutDyingSuccess;
        private SaveNamedBenSuccess saveNamedBenSuccess;
        private SecretRoomFoundSuccess secretRoomFoundSuccess;

        private void Awake()
        {
            dispatcher = Finder.Dispatcher;
        }

        private IEnumerator Start()
        {
            yield return null;
            
            CheckAlreadyUnlockedSuccess();
        }

        /*private void OnEnable()
        {
            firstDeathSuccess.OnFirstDeath += OnFirstDeathDetected;
            wonWithoutDyingSuccess.OnGameWonWithoutDyingSuccess += OnGameWonWithoutDyingWithoutDyingDetected;
        }*/
        
        private void OnDisable()
        {
            firstDeathSuccess.OnFirstDeath -= OnFirstDeathDetected;
            wonWithoutDyingSuccess.OnGameWonWithoutDyingSuccess -= OnGameWonWithoutDyingWithoutDyingDetected;
            wonGameSuccess.OnGameWonSuccess -= OnGameWonDetected;
            saveNamedBenSuccess.OnSaveNamedBen -= OnSaveNamedBenDetected;
            secretRoomFoundSuccess.OnSecretRoomFound -= OnSecretRoomFoundDetected;
        }

        private void OnSecretRoomFoundDetected()
        {
            dispatcher.DataCollector.SecretRoomFound = true;
            secretRoomFoundSuccess.OnSecretRoomFound -= OnSecretRoomFoundDetected;
            secretRoomFoundSuccess.DestroySuccess();
        }

        private void OnFirstDeathDetected()
        {
            dispatcher.DataCollector.FirstDeath = true;
            firstDeathSuccess.OnFirstDeath -= OnFirstDeathDetected;
            firstDeathSuccess.DestroySuccess();
        }

        private void OnGameWonDetected()
        {
            dispatcher.DataCollector.WonGame = true;
            wonGameSuccess.OnGameWonSuccess -= OnGameWonDetected;
            wonGameSuccess.DestroySuccess();
        }
        
        private void OnGameWonWithoutDyingWithoutDyingDetected()
        {
            if (dispatcher.DataCollector.NbDeath == 0)
            {
                dispatcher.DataCollector.WonWithoutDying = true;
                wonWithoutDyingSuccess.OnGameWonWithoutDyingSuccess -= OnGameWonWithoutDyingWithoutDyingDetected;
                wonWithoutDyingSuccess.DestroySuccess();
            }
        }

        private void OnSaveNamedBenDetected()
        {
            if (dispatcher.DataCollector.Name == "ben")
            {
                dispatcher.DataCollector.SaveNamedBen = true;
                saveNamedBenSuccess.OnSaveNamedBen -= OnSaveNamedBenDetected;
                saveNamedBenSuccess.DestroySuccess();
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