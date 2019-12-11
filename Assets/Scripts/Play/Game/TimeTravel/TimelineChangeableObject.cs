using System;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Jeammy Côté and Mathieu Boutet from FPP.
    public class TimelineChangeableObject : MonoBehaviour
    {
        private TimelineChangedEventChannel timelineChangedEventChannel;

        private GameObject primaryTimelineObject;
        private GameObject secondaryTimelineObject;

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            // Needs to be subscribed at the beginning and never unsubscribed
            // because the state has to update even when the object is disabled
            timelineChangedEventChannel.OnTimelineChanged += TimelineChanged;

            GameObject[] children = this.Children();

            foreach (var child in children)
            {
                if (child.CompareTag(R.S.Tag.MainTimeline))
                {
                    primaryTimelineObject = child;
                }

                if (child.CompareTag(R.S.Tag.SecondaryTimeline))
                {
                    secondaryTimelineObject = child;
                }
            }
        }

        private void OnDestroy()
        {
            timelineChangedEventChannel.OnTimelineChanged -= TimelineChanged;
        }

        private void TimelineChanged()
        {
            switch (Finder.TimelineController.CurrentTimeline)
            {
                case Timeline.Primary:
                    primaryTimelineObject?.SetActive(true);
                    secondaryTimelineObject?.SetActive(false);
                    break;
                case Timeline.Secondary:
                    primaryTimelineObject?.SetActive(false);
                    secondaryTimelineObject?.SetActive(true);
                    break;
            }
        }
    }
}