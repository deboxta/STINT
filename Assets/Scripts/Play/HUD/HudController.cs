using System;
using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

//Author : Yannick Cote
namespace Game
{
    [Findable(R.S.Tag.HudController)]
    public class HudController : MonoBehaviour
    {
        [Header("Sanity slider")]
        [SerializeField] private Slider sanitySlider = null;
        
        [Header("Text fields")]
        [SerializeField] private Text primary = null;
        [SerializeField] private Text secondary = null;
        
        [Header("Shade of text fields")]
        [SerializeField] private Text primaryShade;
        [SerializeField] private Text secondaryShade;

        [Header("Years of level")]
        [SerializeField] private string primaryYear = "1990";
        [SerializeField] private string secondaryYear = "1990";
        
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private Player player;
        private TimelineController timelineController;

        

        private const int BIG_FONT_SIZE = 25;
        private const int SMALL_FONT_SIZE = 16;
        
        private void Awake()
        {
            timelineController = Finder.TimelineController;
            player = Finder.Player;
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
        }

        private void Start()
        {
            primary.text = primaryYear;
            primaryShade.text = primaryYear;
            secondary.text = secondaryYear;
            secondaryShade.text = secondaryYear;
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
            SetTimeView(timelineController.CurrentTimeline);
        }

        private void SetTimeView(Timeline timeline)
        {
            switch (timeline)
            {
                case Timeline.Primary:
                    
                    //primary.text = primaryYear;
                    primary.fontSize = BIG_FONT_SIZE;
                    primaryShade.fontSize = BIG_FONT_SIZE;
                    //secondary.text = secondaryYear;
                    secondary.fontSize = SMALL_FONT_SIZE;
                    secondaryShade.fontSize = SMALL_FONT_SIZE;
                    break;
                case Timeline.Secondary:
                    
                    //primary.text = secondaryYear;
                    primary.fontSize = SMALL_FONT_SIZE;
                    primaryShade.fontSize = SMALL_FONT_SIZE;
                    //secondary.text = primaryYear;
                    secondary.fontSize = BIG_FONT_SIZE;
                    secondaryShade.fontSize = BIG_FONT_SIZE;
                    break;
            }
        }

        private void Update()
        {
            UpdateSanityView(CalculateSliderValue());
        }

        private void UpdateSanityView(float position)
        {
            sanitySlider.value = position;
        }
        
        private float CalculateSliderValue()
        {
            return ((player.Vitals.HealthLeft*100) / player.Vitals.MaxMentalHealth);
        }
    }
}
