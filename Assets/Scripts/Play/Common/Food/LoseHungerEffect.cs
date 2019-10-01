using Harmony;
using UnityEngine;

namespace Game
{
    public struct LoseHungerEffect : IEffect
    {
        private readonly float nutritiveValue;

        public LoseHungerEffect(float nutritiveValue)
        {
            this.nutritiveValue = nutritiveValue;
        }

        public void ApplyOn(GameObject gameObject)
        {
            var nutritiveValue = this.nutritiveValue; //Copy for the lambda.
            gameObject.SendMessageToChild<VitalStats>(it => it.Eat(nutritiveValue));
        }
    }
}