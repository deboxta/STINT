using Harmony;
using UnityEngine;

namespace Game
{
    public class TimeChangeableObject : MonoBehaviour
    {
        private TimeChangeEventChannel timeChangeEventChannel;

        private GameObject mainTimelineObject;
        private GameObject secondaryTimelineObject;

        private void Awake()
        {
            timeChangeEventChannel = Finder.TimeChangeEventChannel;
            
            GameObject[] childrens = this.Children();

            foreach (var children in childrens)
            {
                if (children.CompareTag(R.S.Tag.MainTimeline))
                {
                    mainTimelineObject = children;
                }

                if (children.CompareTag(R.S.Tag.SecondaryTimeline))
                {
                    secondaryTimelineObject = children;
                }
            }
        }

        private void OnEnable()
        {
            timeChangeEventChannel.OnTimelineChange += TimelineChanged;
        }

        private void OnDisable()
        {
            timeChangeEventChannel.OnTimelineChange -= TimelineChanged;
        }

        private void TimelineChanged()
        {
            switch (Finder.TimeController.CurrentTimeline)
            {
                case TimelineEnum.Main:
                    mainTimelineObject.SetActive(true);
                    secondaryTimelineObject.SetActive(false);
                    break;
                case TimelineEnum.Secondary:
                    mainTimelineObject.SetActive(false);
                    secondaryTimelineObject.SetActive(true);
                    break;
            }
        }
    }
}