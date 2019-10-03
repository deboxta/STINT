using Harmony;
using UnityEngine;


namespace Game
{
    [Findable(R.S.Tag.TimeController)]
    public class TimeController : MonoBehaviour
    {
        [SerializeField] private KeyCode changeTimeKey = KeyCode.LeftShift;
        
        private TimelineEnum currentTimeline;
        private TimeChangeEventChannel timeChangeEventChannel;
        
        public TimelineEnum CurrentTimeline
        {
            get => currentTimeline;
            private set
            {
                currentTimeline = value;
                timeChangeEventChannel.NotifyTimelineChanged();
            }
        }

        private void Awake()
        {
            timeChangeEventChannel = Finder.TimeChangeEventChannel;
        }

        private void Start()
        {
            CurrentTimeline = TimelineEnum.Main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(changeTimeKey))
            {
                //FlashEffect
                switch (CurrentTimeline)
                {
                    case TimelineEnum.Main:
                        CurrentTimeline = TimelineEnum.Secondary;
                        break;
                    case TimelineEnum.Secondary:
                        CurrentTimeline = TimelineEnum.Main;
                        break;
                }
            }
        }
    }
}