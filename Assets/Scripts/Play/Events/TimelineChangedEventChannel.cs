using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Jeammy Côté and Mathieu Boutet from FPP
    [Findable(R.S.Tag.MainController)]
    public class TimelineChangedEventChannel : MonoBehaviour
    {
        public event TimelineChangedEventHandler OnTimelineChanged;

        public void NotifyTimelineChanged()
        {
            if (OnTimelineChanged != null) OnTimelineChanged();
        }
        
        public delegate void TimelineChangedEventHandler();
    }
}