using UnityEngine;

namespace Game
{
    public class Water : MonoBehaviour, IDrinkable
    {
        [Header("Drinkable")] [SerializeField] private float nutritiveValue = 1f;

        public Vector3 Position => transform.position;

        public IEffect Drink()
        {
            return new LoseThirstEffect(nutritiveValue);
        }
    }
}