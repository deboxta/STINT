using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class SuccessDetector : MonoBehaviour
    {
        private Dispatcher dispatcher;

        private void Awake()
        {
            dispatcher = Finder.Dispatcher;

            CheckAlreadyUnlockedSuccess();
        }

        private void CheckAlreadyUnlockedSuccess()
        {
            if (!dispatcher.DataCollector.FirstDeath)
            {
                
            }
        }
    }
}