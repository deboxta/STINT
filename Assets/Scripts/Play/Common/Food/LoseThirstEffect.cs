using Harmony;
using UnityEngine;

namespace Game
{
    public struct LoseThirstEffect : IEffect
    {
        private readonly float nutritiveValue;

        public LoseThirstEffect(float nutritiveValue)
        {
            this.nutritiveValue = nutritiveValue;
        }

        public void ApplyOn(GameObject gameObject)
        {
            var nutritiveValue = this.nutritiveValue; //Copy for the lambda.
            gameObject.SendMessageToChild<VitalStats>(it => it.Drink(nutritiveValue));
        }
    }
}