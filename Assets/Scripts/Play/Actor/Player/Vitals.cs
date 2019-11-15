using Harmony;
using UnityEngine;

//Author : Yannick Cote
namespace Game
{
    public class Vitals : MonoBehaviour
    {
        [SerializeField] private float maxMentalHealth = 10;
        
        private Player player;
        private TimelineController timelineController;

        private bool isActiveSanity;
        private bool playerIsDead;
        private float healthLeft;

        public float HealthLeft => healthLeft;

        public float MaxMentalHealth => maxMentalHealth;

        private void Awake()
        {
            healthLeft = maxMentalHealth;
            playerIsDead = false;
            isActiveSanity = false;
        }

        private void Start()
        {
            player = Finder.Player;
            timelineController = Finder.TimelineController;
        }

        private void Update()
        {
            if (timelineController.CurrentTimeline == Timeline.Primary)
                isActiveSanity = false;
            else
                isActiveSanity = true;

            if (healthLeft <= 0 && !playerIsDead)
            {
                playerIsDead = true;
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
    }
}