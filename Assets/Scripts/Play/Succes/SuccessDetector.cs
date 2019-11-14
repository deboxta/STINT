using System;
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
        }
    }
}