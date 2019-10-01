using System;
using UnityEngine;

namespace Game
{
    public class Bunny : Animal, IPrey
    {
        [Header("Other")] [SerializeField] [Range(0f, 1f)] private float nutritiveValue = 0.5f;

        public bool IsEatable => !Vitals.IsDead;

        private ISensor<IEatable> foodSensor;
        private ISensor<IDrinkable> drinkSensor;
        private ISensor<IEntity> threatSensor;
        private ISensor<Animal> mateSensor;

        public override ISensor<IEatable> FoodSensor => foodSensor;
        public override ISensor<IDrinkable> DrinkSensor => drinkSensor;
        public override ISensor<IEntity> ThreatSensor => threatSensor;
        public override ISensor<Animal> MateSensor => mateSensor;

        private new void Awake()
        {
            base.Awake();

            var sensor = Sensor;
            foodSensor = sensor.For<IVegetable>();
            drinkSensor = sensor.For<IDrinkable>();
            threatSensor = sensor.For<IPredator>();
            mateSensor = sensor.For<Bunny>();
        }

        [ContextMenu("Eat")]
        public IEffect BeEaten()
        {
            if (!IsEatable)
                throw new Exception("You are trying to eat a dead Bunny. " +
                                    "Check if it is eatable before eating it.");

            Vitals.BeEaten();

            return new LoseHungerEffect(nutritiveValue);
        }
    }
}