using UnityEditor;
using UnityEngine;

namespace Game
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private int horizontalSpeed = 5;
        [SerializeField] private int jumpForce = 10;
        private bool jump;


        private Rigidbody2D rigidBody2D;

        private void Awake()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();
            jump = false;
        }

        public void Move(Vector2 direction)
        {
            var velocity = rigidBody2D.velocity;
            velocity.x = direction.x * horizontalSpeed;
            rigidBody2D.velocity = velocity;
        }

        public void Jump()
        {
            if (jump == false)
            {
                jump = true;
                var velocity = rigidBody2D.velocity;
                velocity.y = jumpForce;
                rigidBody2D.velocity = velocity;
            }
        }
    }
}