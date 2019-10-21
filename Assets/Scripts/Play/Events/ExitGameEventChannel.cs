using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.SceneController)]
    public class ExitGameEventChannel : MonoBehaviour
    {
        public event ExitGameEventHandler OnExitGame;
        
        public void NotifyExitGame()
        { 
            if (OnExitGame != null) OnExitGame();
        }
    
        public delegate void ExitGameEventHandler();
    }
}