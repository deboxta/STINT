using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class SuccessDetector : MonoBehaviour
    {
        private Dispatcher dispatcher;
        private FirstDeathSuccess firstDeathSuccess;
        private WonWithoutDyingSuccess wonWithoutDyingSuccess;

        private void Awake()
        {
            dispatcher = Finder.Dispatcher;

            CheckAlreadyUnlockedSuccess();
        }

        private void OnEnable()
        {
            firstDeathSuccess.OnFirstDeath += OnFirstDeathDetected;
            wonWithoutDyingSuccess.OnGameWonWithoutDyingSuccess += OnGameWonWithoutDyingWithoutDyingDetected;
        }
        
        private void OnDisable()
        {
            firstDeathSuccess.OnFirstDeath -= OnFirstDeathDetected;
        }

        private void OnFirstDeathDetected()
        {
            dispatcher.DataCollector.FirstDeath = true;
            firstDeathSuccess.OnFirstDeath -= OnFirstDeathDetected;
            firstDeathSuccess.DestroySuccess();
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
            }

            if (!dispatcher.DataCollector.WonWithoutDying)
            {
                wonWithoutDyingSuccess = gameObject.AddComponent<WonWithoutDyingSuccess>();
            }
        }
    }
}