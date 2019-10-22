using Harmony;
using UnityEngine;

namespace Game
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private int XSpeed = 10;
        [SerializeField] private int YForce = 15;
        [SerializeField] private int gravityMultiplier = 5;
        [SerializeField] private int fallGravityMultiplier = 10;

        private Rigidbody2D rigidBody2D;
        private bool isGrounded;

        private void Awake()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();
            isGrounded = true;
        }

        private void FixedUpdate()
        {
            if (rigidBody2D.velocity.y < 0)
                rigidBody2D.velocity += Time.deltaTime * Physics2D.gravity.y * gravityMultiplier * Vector2.up;
        }

        public void Move(Vector2 direction)
        {
            var velocity = rigidBody2D.velocity;
            velocity.x = direction.x * XSpeed;
            rigidBody2D.velocity = velocity;
        }

        public void Jump()
        {
            if (isGrounded)
            {
                var velocity = rigidBody2D.velocity;
                velocity.y = YForce;
                rigidBody2D.velocity = velocity;
                isGrounded = false;
            }
        }

        public void Fall()
        {
            rigidBody2D.velocity += Time.deltaTime * Physics2D.gravity.y * fallGravityMultiplier * Vector2.up;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer(R.S.Layer.Floor))
                isGrounded = true;
        }

        public void Slowed()
        {
            XSpeed /= 2;
            YForce /= 2;
        }
        
        public void ResetSpeed()
        {
            XSpeed *= 2;
            YForce *= 2;
        }
    }
}