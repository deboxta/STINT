using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public abstract class OffspringCreator : MonoBehaviour
    {
        [SerializeField] private float reproductionMaxRange = 0.5f;

        private PrefabFactory prefabFactory;
        private Animal animal;
        private GameObject faunaRoot;

        protected PrefabFactory PrefabFactory => prefabFactory;
        protected Animal Animal => animal;
        protected GameObject FaunaRoot => faunaRoot;

        public float ReproductionMaxRange => reproductionMaxRange;

        public event OffspringCreatorEventHandler OnOffspringCreated;

        private void Awake()
        {
            var transformParent = transform.parent;
            prefabFactory = Finder.PrefabFactory;
            animal = transformParent.GetComponent<Animal>();
            faunaRoot = transformParent.parent.gameObject;
        }

        public void CreateOffspringWith(Animal otherAnimal)
        {
            if (!IsInReach(otherAnimal))
                throw new Exception("You are trying to create an offspring with something that is out of reach. " +
                                    "Check if it is in reach before creating an offspring with it.");

            CreateOffspringPrefab(otherAnimal);

            var effect = new LoseReproductiveUrge();
            effect.ApplyOn(animal.gameObject);
            effect.ApplyOn(otherAnimal.gameObject);

            NotifyOffspringCreated();
        }

        public bool IsInReach(Animal otherAnimal)
        {
            //Use a square to compute max distance for performance reasons.
            if (Mathf.Abs(animal.Position.x - otherAnimal.Position.x) <= reproductionMaxRange) return true;
            if (Mathf.Abs(animal.Position.y - otherAnimal.Position.y) <= reproductionMaxRange) return true;
            return false;
        }

        protected abstract Animal CreateOffspringPrefab(Animal otherAnimal);

        private void NotifyOffspringCreated()
        {
            if (OnOffspringCreated != null) OnOffspringCreated();
        }
    }

    public delegate void OffspringCreatorEventHandler();
}