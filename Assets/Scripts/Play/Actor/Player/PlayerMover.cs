using Harmony;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float xSpeed = 10f;
        [SerializeField] private float yForce = 15f;
        [SerializeField] private float gravityMultiplier = 5f;
        [SerializeField] private float fallGravityMultiplier = 10f;
        [SerializeField] private float movementPenality = 2;

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
            velocity.x = direction.x * xSpeed;
            rigidBody2D.velocity = velocity;
        }

        public void Jump()
        {
            if (isGrounded)
            {
                var velocity = rigidBody2D.velocity;
                velocity.y = yForce;
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
            xSpeed /= movementPenality;
            yForce /= movementPenality;
        }
        
        public void ResetSpeed()
        {
            xSpeed *= movementPenality;
            yForce *= movementPenality;
        }
    }
}