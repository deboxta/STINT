using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    [Findable(R.S.Tag.MainController)]
    public class NewGameLoadedEventChannel : MonoBehaviour
    {
        public event NewGameLoadedEventHandler OnNewGameLoaded;
        
        public void NotifyNewGameLoaded()
        {
            if (OnNewGameLoaded != null) OnNewGameLoaded();
        }
        
        public delegate void NewGameLoadedEventHandler();
    }
}