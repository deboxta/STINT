using UnityEngine;

namespace Game
{
    public class VitalStats : MonoBehaviour
    {
        [Header("Initial stats")] [SerializeField] [Range(0f, 1f)] private float initialHunger = 0f;
        [SerializeField] [Range(0f, 1f)] private float initialThirst = 0f;
        [SerializeField] [Range(0f, 1f)] private float initialReproductiveUrge = 0f;
        [Header("Stats consumption")] [SerializeField] private float hungerPerSecond = 0.01f;
        [SerializeField] private float thirstPerSecond = 0.02f;
        [SerializeField] private float reproductiveUrgePerSecond = 0.01f;
        [Header("Warning Thresholds")] [SerializeField] private float hungryThreshold = 0.5f;
        [SerializeField] private float thirstyThreshold = 0.5f;
        [SerializeField] private float hornyThreshold = 0.8f;
        [Header("Death Thresholds")] [SerializeField] private float hungerDeathThreshold = 1f;
        [SerializeField] private float thirstDeathThreshold = 1f;

        private float hunger;
        private float thirst;
        private float reproductiveUrge;
        private DeathReason? deathReason;

        public event VitalStatsEventHandler OnDeath;
        public event VitalStatsEventHandler OnResurected;

        public float Hunger
        {
            get => hunger;
            private set
            {
                if (!IsDead)
                {
                    hunger = Mathf.Clamp01(value);
                    if (hunger >= hungerDeathThreshold) Die(Game.DeathReason.Hunger);
                }
            }
        }

        public float Thirst
        {
            get => thirst;
            private set
            {
                if (!IsDead)
                {
                    thirst = Mathf.Clamp01(value);
                    if (thirst >= thirstDeathThreshold) Die(Game.DeathReason.Thirst);
                }
            }
        }

        public float ReproductiveUrge
        {
            get => reproductiveUrge;
            private set
            {
                if (!IsDead)
                {
                    reproductiveUrge = Mathf.Clamp01(value);
                }
            }
        }

        public DeathReason? DeathReason
        {
            get => deathReason;
            private set
            {
                if (!IsDead)
                {
                    deathReason = value;
                }
            }
        }

        public bool IsHungry => hunger > hungryThreshold;
        public bool IsThirsty => thirst > thirstyThreshold;
        public bool IsHorny => reproductiveUrge > hornyThreshold;
        public bool IsDead => deathReason.HasValue;
        public bool IsHealthy => !IsHungry && !IsThirsty;

        private void Start()
        {
            ResetVitals();
        }

        private void Update()
        {
            if (!IsDead)
            {
                BeHungry(hungerPerSecond * Time.deltaTime);
                BeThirsty(thirstPerSecond * Time.deltaTime);
                BeHorny(reproductiveUrgePerSecond * Time.deltaTime);
            }
        }

        public void Eat(float nutritiveValue)
        {
            Hunger -= nutritiveValue;
        }

        public void Drink(float nutritiveValue)
        {
            Thirst -= nutritiveValue;
        }

        public void HaveSex()
        {
            ReproductiveUrge = 0;
        }

        public void BeHungry(float hungerConsumption)
        {
            Hunger += hungerConsumption;
        }
        
        public void BeThirsty(float thirstConsumption)
        {
            Thirst += thirstConsumption;
        }

        public void BeHorny(float reproductiveUrgeConsumption)
        {
            ReproductiveUrge += reproductiveUrgeConsumption;
        }

        public void BeEaten()
        {
            Die(Game.DeathReason.Eaten);
        }

        private void Die(DeathReason deathReason)
        {
            DeathReason = deathReason;
            NotifyDeath();
        }

        public void Resurect()
        {
            ResetVitals();
            NotifyResurected();
        }

        private void ResetVitals()
        {
            Hunger = initialHunger;
            Thirst = initialThirst;
            ReproductiveUrge = initialReproductiveUrge;
            DeathReason = null;
        }

        private void NotifyDeath()
        {
            if (OnDeath != null) OnDeath();
        }

        private void NotifyResurected()
        {
            if (OnResurected != null) OnResurected();
        }
    }

    public enum DeathReason
    {
        Hunger,
        Thirst,
        Eaten,
    }

    public delegate void VitalStatsEventHandler();
}