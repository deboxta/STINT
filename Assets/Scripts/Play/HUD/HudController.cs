
using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Slider sanitySlider = null;
        [SerializeField] private float travelMaxTime = 0;
        
        [SerializeField] private Text primary = null;
        [SerializeField] private Text secondary = null;
    
        private Player player;
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private bool isActiveSanity;
        private float timeLeft;

        
        void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            player = Finder.Player;
            isActiveSanity = false;
            timeLeft = travelMaxTime;
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
                    
                    primary.text = "1984";
                    primary.fontSize = 32;
                    secondary.text = "3024";
                    secondary.fontSize = 16;
                break;
                case Timeline.Secondary:
                    isActiveSanity = true;
                    
                    primary.text = "3024";
                    primary.fontSize = 16;
                    secondary.text = "1984";
                    secondary.fontSize = 32;
                break;
            }
        }


        void Update()
        {
            sanitySlider.value = CalculateSliderValue();
            if (timeLeft <= 0)
            {
                Time.timeScale = travelMaxTime;
                player.Die();
            }
            else if (timeLeft > 0)
            {
                if (isActiveSanity)
                    timeLeft -= Time.deltaTime;
                else
                    timeLeft += Time.deltaTime;
            }
        }

        private float CalculateSliderValue()
        {
            return ((timeLeft*100) / travelMaxTime);
        }
    }
}
