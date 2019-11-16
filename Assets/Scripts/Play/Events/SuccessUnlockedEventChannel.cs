using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class SuccessUnlockedEventChannel : MonoBehaviour
    {
        public event SuccessUnlockedEventHandler OnSuccessUnlocked;
        
        public void NotifySuccessUnlocked(string successName)
        {
            if (OnSuccessUnlocked != null) OnSuccessUnlocked(successName);
        }
        
        public delegate void SuccessUnlockedEventHandler(string successName);
    }
}