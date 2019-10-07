﻿using Harmony;
using UnityEngine;

namespace Game
{
    public class TimelineChangeableObject : MonoBehaviour
    {
        private TimelineChangedEventChannel timelineChangedEventChannel;

        private GameObject mainTimelineObject;
        private GameObject secondaryTimelineObject;

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            
            GameObject[] children = this.Children();

            foreach (var child in children)
            {
                if (child.CompareTag(R.S.Tag.MainTimeline))
                {
                    mainTimelineObject = child;
                }

                if (child.CompareTag(R.S.Tag.SecondaryTimeline))
                {
                    secondaryTimelineObject = child;
                }
            }
        }

        private void OnEnable()
        {
            timelineChangedEventChannel.OnTimelineChanged += TimelineChanged;
        }

        private void OnDisable()
        {
            timelineChangedEventChannel.OnTimelineChanged -= TimelineChanged;
        }

        private void TimelineChanged()
        {
            switch (Finder.TimelineController.CurrentTimeline)
            {
                case Timeline.Main:
                    mainTimelineObject.SetActive(true);
                    secondaryTimelineObject.SetActive(false);
                    break;
                case Timeline.Secondary:
                    mainTimelineObject.SetActive(false);
                    secondaryTimelineObject.SetActive(true);
                    break;
            }
        }
    }
}