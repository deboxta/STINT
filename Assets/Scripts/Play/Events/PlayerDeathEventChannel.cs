using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    [Findable(R.S.Tag.MainController)]
    public class PlayerDeathEventChannel : MonoBehaviour
    {
        public event PlayerDeathEventHandler OnPlayerDeath;
        
        public void NotifyPlayerDeath() 
        { 
            if (OnPlayerDeath != null) OnPlayerDeath();
        }
        
        public delegate void PlayerDeathEventHandler();
    }
}