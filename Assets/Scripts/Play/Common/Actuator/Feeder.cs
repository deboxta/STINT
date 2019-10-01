using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public sealed class Feeder : MonoBehaviour
    {
        [SerializeField] private float maxEatDistance = 0.5f;
        [SerializeField] private float maxDrinkDistance = 0.5f;

        private IEntity entity;

        private void Awake()
        {
            entity = transform.parent.GetComponent<IEntity>();
        }

        public void Eat(IEatable eatable)
        {
            if (!IsInReach(eatable))
                throw new Exception("You are trying to eat something that is out of reach. " +
                                    "Check if it is in reach before eating it.");

            eatable.BeEaten().ApplyOn(transform.parent.gameObject);
        }

        public void Drink(IDrinkable drinkable)
        {
            if (!IsInReach(drinkable))
                throw new Exception("You are trying to drink something that is out of reach. " +
                                    "Check if it is in reach before eating it.");
            drinkable.Drink().ApplyOn(transform.parent.gameObject);
        }

        public bool IsInReach(IEatable eatable)
        {
            return IsInReach(eatable, maxEatDistance);
        }

        public bool IsInReach(IDrinkable eatable)
        {
            return IsInReach(eatable, maxDrinkDistance);
        }

        private bool IsInReach(IEntity eatable, float maxDistance)
        {
            return entity.Position.SqrDistanceTo(eatable.Position) < maxDistance * maxDistance;
        }
    }
}