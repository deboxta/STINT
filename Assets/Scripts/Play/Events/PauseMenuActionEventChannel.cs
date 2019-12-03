using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class PauseMenuActionEventChannel : MonoBehaviour
    {
        public event PauseMenuActionEventHandler OnPauseMenuAction;
    
        public void NotifyPauseMenuAction()
        {
            if (OnPauseMenuAction != null) OnPauseMenuAction();
        }
    
        public delegate void PauseMenuActionEventHandler();
    }
}