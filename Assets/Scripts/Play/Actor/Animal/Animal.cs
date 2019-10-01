using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public abstract class Animal : Actor
    {
        [Header("State : Searching")] [SerializeField] private int searchMinSteps = 1;
        [SerializeField] private int searchMaxSteps = 25;

        private VitalStats vitals;
        private Mover mover;
        private PathFinder pathFinder;
        private Feeder feeder;
        private OffspringCreator offspringCreator;
        private Sensor sensor;

        public int SearchMinSteps => searchMinSteps;
        public int SearchMaxSteps => searchMaxSteps;

        public VitalStats Vitals => vitals;
        public Mover Mover => mover;
        public PathFinder PathFinder => pathFinder;
        public Feeder Feeder => feeder;
        public OffspringCreator OffspringCreator => offspringCreator;
        protected Sensor Sensor => sensor;

        public abstract ISensor<IEatable> FoodSensor { get; }
        public abstract ISensor<IDrinkable> DrinkSensor { get; }
        public abstract ISensor<IEntity> ThreatSensor { get; }
        public abstract ISensor<Animal> MateSensor { get; }

        protected void Awake()
        {
            pathFinder = Finder.PathFinder;

            vitals = GetComponentInChildren<VitalStats>();
            mover = GetComponentInChildren<Mover>();
            feeder = GetComponentInChildren<Feeder>();
            offspringCreator = GetComponentInChildren<OffspringCreator>();
            sensor = GetComponentInChildren<Sensor>();
        }

        private void Update()
        {
#if UNITY_EDITOR
            try
            {
#endif
                //StateMachine.Update();
#if UNITY_EDITOR
            }
            catch (Exception ex)
            {
                Debug.LogError($"{name} errored : {ex.Message}\n{ex.StackTrace}.", gameObject);
                gameObject.SetActive(false);
            }
#endif
        }

        private void OnEnable()
        {
            vitals.OnDeath += OnDeath;

            if (vitals.IsDead) OnDeath();
        }

        private void OnDisable()
        {
            vitals.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            Destroy();
        }

        [ContextMenu("Destroy")]
        private void Destroy()
        {
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            //StateMachine?.DrawDebugInfo();
        }
#endif
    }
}