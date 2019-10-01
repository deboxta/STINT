using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Grass : Actor, IVegetable
    {
        [Header("Growth")] [SerializeField] [Range(0f, 25f)] private float eatDuration = 1f;
        [SerializeField] [Range(0f, 25f)] private float growthDelay = 10f;
        [SerializeField] [Range(0f, 25f)] private float growthDuration = 3f;
        [SerializeField] private Vector3 eatenScale = new Vector3(0.1f, 0.1f, 0.1f);
        [Header("Other")] [SerializeField] [Range(0f, 1f)] private float nutritiveValue = 0.5f;

        private Vector3 initialScale;

        public bool IsEatable { get; private set; }

        private void Start()
        {
            initialScale = transform.localScale;
            IsEatable = true;
        }

        [ContextMenu("Eat")]
        public IEffect BeEaten()
        {
            if (!IsEatable)
                throw new Exception("You are trying to eat grass not yet grown. " +
                                    "Check if it is eatable before eating it.");

            StartCoroutine(EatAndGrowRoutine(growthDelay));

            return new LoseHungerEffect(nutritiveValue);
        }

        private IEnumerator EatAndGrowRoutine(float delay)
        {
            IsEatable = false;
            yield return DOTween.Sequence()
                .Append(transform.DOScale(eatenScale, eatDuration))
                .AppendInterval(delay)
                .Append(transform.DOScale(initialScale, growthDuration))
                .WaitForCompletion();
            IsEatable = true;
        }
    }
}