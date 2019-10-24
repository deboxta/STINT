using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Slider sanitySlider = null;
        [SerializeField] private Text primary = null;
        [SerializeField] private string primaryYear = "1990";
        [SerializeField] private string secondaryYear = "1990";
        [SerializeField] private Text secondary = null;
    
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private Player player;

        private void Start()
        {
            player = Finder.Player;
        }

        void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
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
                case Timeline.Primary:
                    
                    primary.text = primaryYear;
                    primary.fontSize = 32;
                    secondary.text = secondaryYear;
                    secondary.fontSize = 16;
                break;
                case Timeline.Secondary:
                    
                    primary.text = secondaryYear;
                    primary.fontSize = 16;
                    secondary.text = primaryYear;
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
