using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class FirstDeathSuccess : MonoBehaviour
    {
        public event FirstDeathSuccessEventHandler OnFirstDeath;
        
        private PlayerDeathEventChannel playerDeathEventChannel;

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
        }

        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += NotifyFirstDeath;
        }

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= NotifyFirstDeath;
        }

        public void NotifyFirstDeath() 
        { 
            if (OnFirstDeath != null) OnFirstDeath();
        }

        public void DestroySuccess()
        {
            Destroy(this);
        }
        
        public delegate void FirstDeathSuccessEventHandler();
    }
}