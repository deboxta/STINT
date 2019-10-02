using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.TimeController)]
    public class TimeChangeEventHandler : MonoBehaviour
    {
        public event TimelineChangeEventHandler OnTimelineChange;

        public void NotifyTimelineChanged()
        {
            if (OnTimelineChange != null) OnTimelineChange();
        }
        
        public delegate void TimelineChangeEventHandler();
    }
}