using Harmony;
using UnityEngine;
using UnityEngine.UI;

//Author : Yannick Cote
namespace Game
{
    [Findable(R.S.Tag.HudController)]
    public class HudController : MonoBehaviour
    {
        //BR : Good!
        [Header("Sanity slider")]
        [SerializeField] private Slider sanitySlider = null;
        
        [Header("Text fields")]
        [SerializeField] private Text primary = null;
        [SerializeField] private Text secondary = null;

        [Header("Years of level")]
        [SerializeField] private string primaryYear = "1990";
        [SerializeField] private string secondaryYear = "1990";
    
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private Player player;

        //BR : Pourquoi est-ce fait au "Start" au lieu du "Awake" ? Je vois pas de raison.
        private void Start()
        {
            player = Finder.Player;
        }

        //BC : Private manquant. Innacceptable en 5e session depuis le temps que je le dis.
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

        //BR : Good.
        private void TimelineChange()
        {
            //BC : Devrait obtenir le "TimelineController" au Awake et le conserver.
            //     Mauvaise pratique.
            //BR : Tu as mis la mise à jour du "SanitySlider" dans une méthode à part.
            //     Tu pourrais faire la même chose pour ça aussi.
            switch (Finder.TimelineController.CurrentTimeline)
            {
                case Timeline.Primary:
                    
                    primary.text = primaryYear;
                    primary.fontSize = 32; //BC : Chiffre magique.
                    secondary.text = secondaryYear;
                    secondary.fontSize = 16; //BC : Chiffre magique.
                break;
                case Timeline.Secondary:
                    
                    primary.text = secondaryYear;
                    primary.fontSize = 16; //BC : Chiffre magique.
                    secondary.text = primaryYear;
                    secondary.fontSize = 32; //BC : Chiffre magique.
                break;
            }
        }

        private void Update()
        {
            SetSanitySlider(player.Vitals.CalculateSliderValue());
        }

        //BC : Warning ignoré + devrait être private.
        //BR : J'aurais appellé ça "UpdateSanityView".
        public void SetSanitySlider(float position)
        {
            sanitySlider.value = position;
        }
    }
}
