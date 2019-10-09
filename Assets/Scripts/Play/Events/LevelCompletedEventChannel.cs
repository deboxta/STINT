using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class LevelCompletedEventChannel : MonoBehaviour
    {
        public event LevelCompletedEventHandler OnLevelCompleted;
        
        public void NotifyLevelCompleted() 
        { 
            if (OnLevelCompleted != null) OnLevelCompleted();
        }
        
        public delegate void LevelCompletedEventHandler();
    }
}