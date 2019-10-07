using UnityEngine;

namespace Game
{
    public class Box : MonoBehaviour
    {
        protected Rigidbody2D rigidBody2D;
        protected Collider2D boxCollider2D;
        protected SpriteRenderer spriteRenderer;
        protected Stimuli stimuli;
        
        private void Awake()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();
            boxCollider2D = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            stimuli = GetComponentInChildren<Stimuli>();
        }
        
        public Rigidbody2D GetRigidBody2D()
        {
            return rigidBody2D;
        }
    }
}