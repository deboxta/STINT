﻿using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    //Author : Jeammy Côté and Mathieu Boutet from FPP
    [Findable(R.S.Tag.MainController)]
    public class TimelineController : MonoBehaviour
    {
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
            CurrentTimeline = Timeline.Primary;
        }
        public void ResetTimeline()
        {
            CurrentTimeline = Timeline.Primary;
        }

        public void SwitchTimeline()
        {
            switch (CurrentTimeline)
            {
                case Timeline.Primary:
                    CurrentTimeline = Timeline.Secondary;
                    break;
                case Timeline.Secondary:
                    CurrentTimeline = Timeline.Primary;
                    break;
            }
        }
    }
}