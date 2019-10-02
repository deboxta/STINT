using Harmony;
using UnityEngine;


namespace Game
{
    [Findable(R.S.Tag.TimeController)]
    public class TimeController : MonoBehaviour
    {
        [SerializeField] private KeyCode changeTimeKey = KeyCode.LeftShift;
        
        private TimelineEnum currentTimeline;
        private TimeChangeEventHandler timeChangeEventHandler;
        
        public TimelineEnum CurrentTimeline
        {
            get => currentTimeline;
            private set
            {
                currentTimeline = value;
                timeChangeEventHandler.NotifyTimelineChanged();
            }
        }

        private void Awake()
        {
            timeChangeEventHandler = Finder.TimeChangeEventHandler;
        }

        private void Start()
        {
            currentTimeline = TimelineEnum.Main;
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