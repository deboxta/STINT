
using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Image damageImage;
        [SerializeField] private Slider sanitySlider;
        [SerializeField] private int travelMaxTime;
    
        private Player player;
        private TimelineChangedEventChannel timelineChangedEventChannel;

        private bool damageTreshold;
    
        void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            sanitySlider.value = travelMaxTime;
            player = GetComponent<Player>();
        }

        public void DecreaseSanityByTime()
        {
            StartCoroutine(DecreaseSlider());
        }

        IEnumerator DecreaseSlider()
        {
            if(sanitySlider != null)
            {
                float timeSlice = (sanitySlider.value / 10);
                while (sanitySlider.value >= 0)
                {
                    sanitySlider.value -= timeSlice;
                    yield return new WaitForSeconds(1);
                    if (sanitySlider.value <= 0)
                        break;
                }
            }
            yield return null;
        }

        private void OnEnable()
        {
            timelineChangedEventChannel.OnTimelineChanged += TimelineChange;
        }

        private void OnDisable()
        {
            timelineChangedEventChannel.OnTimelineChanged -= TimelineChange;
        }

        private void TimelineChange()
        {
            switch (Finder.TimelineController.CurrentTimeline)
            {
                case Timeline.Main:

                break;
                case Timeline.Secondary:
                    DecreaseSanityByTime();
                break;
            }
        }


        void Update()
        {
        }
    }
}
