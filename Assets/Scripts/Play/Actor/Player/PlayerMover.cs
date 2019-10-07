using UnityEditor;
using UnityEngine;

namespace Game
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private int horizontalSpeed = 5;
        [SerializeField] private int jumpForce = 10;
        [SerializeField] private int gravityMultiplier = 1;
        [SerializeField] private int fallGravityMultiplier = 5;
        

        private Rigidbody2D rigidBody2D;

        private void Awake()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();
        }
        
        private void FixedUpdate()
        {
            if (rigidBody2D.velocity.y < 0)
                rigidBody2D.velocity += Time.deltaTime * Physics2D.gravity.y * gravityMultiplier * Vector2.up;
        }

        public void Move(Vector2 direction)
        {
            var velocity = rigidBody2D.velocity;
            velocity.x = direction.x * horizontalSpeed;
            rigidBody2D.velocity = velocity;
        }

        public void Jump()
        {
            var velocity = rigidBody2D.velocity;
            velocity.y = jumpForce;
            rigidBody2D.velocity = velocity;
        }
        
        public void Fall()
        {
            rigidBody2D.velocity += Time.deltaTime * Physics2D.gravity.y * fallGravityMultiplier * Vector2.up;
        }
    }
}