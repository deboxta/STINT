
using System;
using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Slider sanitySlider = null;
        [SerializeField] private Text primary = null;
        [SerializeField] private Text secondary = null;
    
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private bool isActiveSanity;
        private Player player;

        private void Start()
        {
            player = Finder.Player;
        }

        void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            isActiveSanity = false;
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

        private void Update()
        {
            SetSanitySlider(player.Vitals.CalculateSliderValue());
        }

        public void SetSanitySlider(float position)
        {
            sanitySlider.value = position;
        }
    }
}
