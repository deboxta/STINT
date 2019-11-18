using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    [Findable(R.S.Tag.MainController)]
    public class GameWonEventChannel : MonoBehaviour
    {
        public event GameWonEventHandler OnGameWon;
        
        public void NotifyGameWon() 
        { 
            if (OnGameWon != null) OnGameWon();
        }
        
        public delegate void GameWonEventHandler();
    }
}