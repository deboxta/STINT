using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class PlayerHitEventChannel : MonoBehaviour
    {
        public event PlayerHitEventHandler OnPlayerHit;
        
        public void NotifyPlayerHit() 
        { 
            if (OnPlayerHit != null) OnPlayerHit();
        }
        
        public delegate void PlayerHitEventHandler();
    }
}