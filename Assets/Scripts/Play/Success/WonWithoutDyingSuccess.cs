using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class WonWithoutDyingSuccess : MonoBehaviour, ISuccess
    {
        public event GameWonSuccessEventHandler OnGameWonWithoutDyingSuccess;
        
        private GameWonEventChannel gameWonEventChannel;
        
        public string successName { get; set; }

        private void Awake()
        {
            gameWonEventChannel = Finder.GameWonEventChannel;
            
            successName = "Won Without Dying Success";
        }

        private void OnEnable()
        {
            gameWonEventChannel.OnGameWon += NotifyGameWonWithoutDying;
        }

        private void OnDisable()
        {
            gameWonEventChannel.OnGameWon -= NotifyGameWonWithoutDying;
        }

        private void NotifyGameWonWithoutDying() 
        { 
            if (OnGameWonWithoutDyingSuccess != null) OnGameWonWithoutDyingSuccess();
        }

        public void DestroySuccess()
        {
            Destroy(this);
        }
        
        public delegate void GameWonSuccessEventHandler();
    }
}