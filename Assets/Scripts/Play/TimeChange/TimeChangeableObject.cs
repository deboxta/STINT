using Harmony;
using UnityEngine;

namespace Game
{
    public class TimeChangeableObject : MonoBehaviour
    {
        private TimeChangeEventHandler timeChangeEventHandler;

        private GameObject mainTimelineObject;
        private GameObject secondaryTimelineObject;

        private void Awake()
        {
            timeChangeEventHandler = Finder.TimeChangeEventHandler;
            
            GameObject[] childrens = this.Children();

            foreach (var children in childrens)
            {
                if (children.layer.ToString() == R.S.Layer.MainTimeline)
                {
                    mainTimelineObject = children;
                }

                if (children.layer.ToString() == R.S.Layer.SecondaryTimeline)
                {
                    secondaryTimelineObject = children;
                }
            }
        }

        private void OnEnable()
        {
            timeChangeEventHandler.OnTimelineChange += TimelineChanged;
        }

        private void OnDisable()
        {
            timeChangeEventHandler.OnTimelineChange -= TimelineChanged;
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