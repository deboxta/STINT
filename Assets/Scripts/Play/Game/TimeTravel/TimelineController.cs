using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class TimelineController : MonoBehaviour
    {
        private const KeyCode CHANGE_TIMELINE_KEYBOARD_KEY = KeyCode.LeftShift;
        
        private Timeline currentTimeline;
        private TimelineChangedEventChannel timelineChangedEventChannel;
        
        public Timeline CurrentTimeline
        {
            get => currentTimeline;
            private set
            {
                currentTimeline = value;
                timelineChangedEventChannel.NotifyTimelineChanged();
            }
        }

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
        }

        private void Start()
        {
            CurrentTimeline = Timeline.Main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(CHANGE_TIMELINE_KEYBOARD_KEY) 
                || GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed 
                || GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)
            {
                Finder.FlashEffect.Flash();
                switch (CurrentTimeline)
                {
                    case Timeline.Main:
                        CurrentTimeline = Timeline.Secondary;
                        break;
                    case Timeline.Secondary:
                        CurrentTimeline = Timeline.Main;
                        break;
                }
            }
        }
    }
}