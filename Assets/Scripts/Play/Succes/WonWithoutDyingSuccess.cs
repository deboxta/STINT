using Harmony;
using UnityEngine;

namespace Game
{
    public class WonWithoutDyingSuccess : MonoBehaviour
    {
        public event GameWonSuccessEventHandler OnGameWonWithoutDyingSuccess;
        
        private GameWonEventChannel gameWonEventChannel;

        private void Awake()
        {
            gameWonEventChannel = Finder.GameWonEventChannel;
        }

        private void OnEnable()
        {
            gameWonEventChannel.OnGameWon += NotifyGameWonWithoutDying;
        }

        private void OnDisable()
        {
            gameWonEventChannel.OnGameWon -= NotifyGameWonWithoutDying;
        }

        public void NotifyGameWonWithoutDying() 
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