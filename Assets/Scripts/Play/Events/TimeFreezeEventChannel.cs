using System;
using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class TimeFreezeEventChannel : MonoBehaviour
    {
        public event TimeFreezeEventHandler OnTimeFreezeStateChanged;
        
        public void NotifyTimeFreezeStateChanged()
        {
            Debug.Log("Time frozen: " + Finder.TimeFreezeController.IsFrozen);
            if (OnTimeFreezeStateChanged != null) OnTimeFreezeStateChanged();
        }
        
        public delegate void TimeFreezeEventHandler();
    }
}