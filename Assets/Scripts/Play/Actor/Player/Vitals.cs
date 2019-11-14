using Harmony;
using UnityEngine;

//Author : Yannick Cote
namespace Game
{
    public class Vitals : MonoBehaviour
    {
        [SerializeField] private float maxMentalHealth = 10;
        
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private PlayerDeathEventChannel deathEventChannel;
        private Player player;

        private bool isActiveSanity;
        private float healthLeft;
        
        private void Awake()
        {
            deathEventChannel = Finder.PlayerDeathEventChannel;
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            isActiveSanity = false;
            healthLeft = maxMentalHealth;
        }

        private void Start()
        {
            player = Finder.Player;
            
        }

        private void OnEnable()
        {
            timelineChangedEventChannel.OnTimelineChanged += TimelineChange;
            deathEventChannel.OnPlayerDeath += PlayerDeath;
        }


        private void OnDisable()
        {
            timelineChangedEventChannel.OnTimelineChanged -= TimelineChange;
            deathEventChannel.OnPlayerDeath -= PlayerDeath;
        }
        
        //BC : Le joueur meurt et résussite tout de suite après ?
        //     Vous "reloadez" votre scène à chaque mort. Ça sert à rien ça.
        private void PlayerDeath()
        {
            healthLeft = maxMentalHealth;
        }
        
        private void TimelineChange()
        {
            switch (Finder.TimelineController.CurrentTimeline)
            {
                //BR: Pourquoi alors ne pas tout simplement utiliser "TimelineController" dans
                //    la méthode "Update" et comparer la valeur de "CurrentTimeline" ?
                //
                //    Cela éviterait d'utiliser le canal événementiel.
                //
                //    À moins de vouloir créer une coroutine quand la Timeline change ? À ce moment
                //    là, ça va.
                case Timeline.Primary:
                    isActiveSanity = false;
                    break;
                case Timeline.Secondary:
                    isActiveSanity = true;
                    break;
            }
        }

        private void Update()
        {
            if (healthLeft <= 0)
            {
                //BC : Le joueur va mourir combien de fois ?
                //     Actuellement, il meurt à chaque frame. Cela devrait se produire une seule fois.
                player.Die();
            }
            else if (healthLeft > 0)
            {
                if (isActiveSanity)
                    healthLeft -= Time.deltaTime;
                else
                    healthLeft += Time.deltaTime;
            }
        }
        
        //BC : Pas à la bonne place. Devrait être fait dans "HudController" à la place.
        public float CalculateSliderValue()
        {
            return ((healthLeft*100) / maxMentalHealth);
        }
    }
}