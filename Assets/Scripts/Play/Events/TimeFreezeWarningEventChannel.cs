using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    [Findable(R.S.Tag.MainController)]
    public class TimeFreezeWarningEventChannel : MonoBehaviour
    {
        public event TimeFreezeWarningEventHandler OnTimeFreezeWarning;
        
        public void NotifyTimeFreezeWarning()
        {
            if (OnTimeFreezeWarning != null) OnTimeFreezeWarning();
        }
        
        public delegate void TimeFreezeWarningEventHandler();
    }
}