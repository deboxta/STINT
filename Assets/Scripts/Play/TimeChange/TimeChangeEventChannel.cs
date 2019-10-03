using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.TimeController)]
    public class TimeChangeEventChannel : MonoBehaviour
    {
        public event TimelineChangeEventChannel OnTimelineChange;

        public void NotifyTimelineChanged()
        {
            if (OnTimelineChange != null) OnTimelineChange();
        }
        
        public delegate void TimelineChangeEventChannel();
    }
}