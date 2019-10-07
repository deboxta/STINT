
using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Image damageImage = null;
        [SerializeField] private Slider sanitySlider = null;
        [SerializeField] private int travelMaxTime = 0;
        [SerializeField] private int timeToWait = 0;
    
        private Player player;
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private bool isActiveSanity;
        
        void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            player = Finder.Player;
            DecreaseSanityByTime();
        }

        public void DecreaseSanityByTime()
        {
            StartCoroutine(DecreaseSlider());
        }

        IEnumerator DecreaseSlider()
        {
            if(sanitySlider != null)
            {
                float timeBySection = (sanitySlider.value / travelMaxTime);
                while (sanitySlider.value >= 0)
                {
                    if (isActiveSanity)
                        sanitySlider.value -= 1;
                    else
                        sanitySlider.value += 1;
                    yield return new WaitForSeconds(1);
                    if (sanitySlider.value <= 0)
                        break;
                }
                player.Die();
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
                    isActiveSanity = false;
                break;
                case Timeline.Secondary:
                    isActiveSanity = true;
                break;
            }
        }


        void Update()
        {
            
        }
    }
}
