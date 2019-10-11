using System;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Vitals : MonoBehaviour
    {
        [SerializeField] private float maxMentalHealth = 0;
        
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private bool isActiveSanity;
        private float healthLeft;
        private Player player;

        public float HealthLeft
        {
            get => healthLeft;
        }

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            isActiveSanity = false;
            healthLeft = maxMentalHealth;
        }

        private void Start()
        {
            player = GetComponent<Player>();
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