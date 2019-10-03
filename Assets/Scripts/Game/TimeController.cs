using Harmony;
using UnityEngine;
using XInputDotNetPure;


namespace Game
{
    [Findable(R.S.Tag.TimeController)]
    public class TimeController : MonoBehaviour
    {
        [SerializeField] private KeyCode changeTimeKeyboardKey = KeyCode.LeftShift;
        [SerializeField] private ButtonState changeTimeGamePadButton = GamePad.GetState(PlayerIndex.One).Buttons.X;
        [SerializeField] private ButtonState secondaryChangeTimeGamePadButton = GamePad.GetState(PlayerIndex.One).Buttons.Y;
        
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
            if (Input.GetKeyDown(changeTimeKeyboardKey) 
                || GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed 
                || GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)
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