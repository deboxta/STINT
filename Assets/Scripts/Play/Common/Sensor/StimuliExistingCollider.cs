using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    // Takes an already existing Collider2D in the object instead of creating a new one
    public sealed class StimuliExistingCollider : Stimuli
    {
        protected override void Awake()
        {
            base.Awake();
            
            InitCollider();
        }

        private void InitCollider()
        {
            gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }
}