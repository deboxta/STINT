using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class FirstDeathSuccess : MonoBehaviour, ISuccess
    {
        public event FirstDeathSuccessEventHandler OnFirstDeath;
        
        private PlayerDeathEventChannel playerDeathEventChannel;
        
        public string successName { get; set; }

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            successName = "First Death Success";
        }

        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += NotifyFirstDeath;
        }

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= NotifyFirstDeath;
        }

        private void NotifyFirstDeath() 
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