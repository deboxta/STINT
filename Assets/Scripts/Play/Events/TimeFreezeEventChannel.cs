using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    [Findable(R.S.Tag.MainController)]
    public class TimeFreezeEventChannel : MonoBehaviour
    {
        public event TimeFreezeEventHandler OnTimeFreezeStateChanged;
        
        public void NotifyTimeFreezeStateChanged()
        {
            if (OnTimeFreezeStateChanged != null) OnTimeFreezeStateChanged();
        }
        
        public delegate void TimeFreezeEventHandler();
    }
}