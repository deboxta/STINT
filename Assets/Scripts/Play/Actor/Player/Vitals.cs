using System;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Vitals : MonoBehaviour
    {
        [SerializeField] private float maxMentalHealth = 10;
        
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private PlayerDeathEventChannel deathEventChannel;
        private bool isActiveSanity;
        private float healthLeft;
        private Player player;

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
        
        private void PlayerDeath()
        {
            healthLeft = maxMentalHealth;
        }
        
        private void TimelineChange()
        {
            switch (Finder.TimelineController.CurrentTimeline)
            {
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
        
        public float CalculateSliderValue()
        {
            return ((healthLeft*100) / maxMentalHealth);
        }
    }
}