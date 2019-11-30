using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class WonGameSuccess : MonoBehaviour, ISuccess
    {
        public event GameWonSuccessEventHandler OnGameWonSuccess;
        
        private GameWonEventChannel gameWonEventChannel;
        
        public string successName { get; set; }

        private void Awake()
        {
            gameWonEventChannel = Finder.GameWonEventChannel;
            
            successName = "Won Game Success";
        }

        private void OnEnable()
        {
            gameWonEventChannel.OnGameWon += NotifyGameWon;
        }

        private void OnDisable()
        {
            gameWonEventChannel.OnGameWon -= NotifyGameWon;
        }

        private void NotifyGameWon() 
        { 
            if (OnGameWonSuccess != null) OnGameWonSuccess();
        }

        public void DestroySuccess()
        {
            Destroy(this);
        }
        
        public delegate void GameWonSuccessEventHandler();
    }
}