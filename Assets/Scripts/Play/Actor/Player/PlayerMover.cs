using System;
using Harmony;
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
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isTouchingWall;
        [SerializeField] private int numberOfJumps = 3;
        [SerializeField] private float wallJumpForce;
        [SerializeField] private float wallDistance = 1f;
        [SerializeField] private float groundDistance = 1f;
        [SerializeField] private Transform groundCheck; 
        [SerializeField] private Transform wallCheck; 
        [SerializeField] private float groundCheckRadius; 
        [SerializeField] private LayerMask floorLayer;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private bool canJump;
        [SerializeField] private int numberOfJumpsLeft;
        [SerializeField]private bool isWallSliding;
        
        private Rigidbody2D rigidBody2D;
        
        private Vector2 wallJumpDirection;

        public bool IsGrounded => isGrounded;
        public bool IsTouchingWall => isTouchingWall;

        private void Awake()
        {
            wallJumpDirection.Normalize();
            rigidBody2D = GetComponent<Rigidbody2D>();
            wallLayer = LayerMask.NameToLayer(R.S.Layer.Wall);
            //floorLayer = LayerMask.NameToLayer(R.S.Layer.Floor);
        }

        private void Update()
        {
            CheckIfCanJump();
            CheckIfWallSliding();
            CheckSurroundings();
        }

        private void FixedUpdate()
        {
            if (rigidBody2D.velocity.y < 0)
                rigidBody2D.velocity += Time.deltaTime * Physics2D.gravity.y * gravityMultiplier * Vector2.up;
        }

        private void CheckSurroundings()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,floorLayer);
            isTouchingWall = Physics2D.Raycast(wallCheck.position, Vector2.right * transform.localScale.x, wallDistance, floorLayer);
        }

        public void Move(Vector2 direction)
        {
            var velocity = rigidBody2D.velocity;
            velocity.x = direction.x * horizontalSpeed;
            rigidBody2D.velocity = velocity;
        }

        public void Jump()
        {
            if (canJump && !isWallSliding)
            {
                rigidBody2D.velocity = new Vector2(x: rigidBody2D.velocity.x, jumpForce);
                numberOfJumpsLeft--;
            }
            else if (canJump && (isWallSliding || isTouchingWall))
            {
                isWallSliding = false;
                numberOfJumpsLeft--;
                Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x , wallJumpForce * wallJumpDirection.y);
                rigidBody2D.AddForce(forceToAdd,ForceMode2D.Impulse);
            }
        }

        private void CheckIfWallSliding()
        {
            if (isTouchingWall && !isGrounded)
            {
                isWallSliding = true;
            }
            else
            {
                isWallSliding = false;
            }
        }
        
        private void CheckIfCanJump()
        {
            if (isGrounded && rigidBody2D.velocity.y <= 0)
            {
                numberOfJumpsLeft = numberOfJumps;
            }

            if (numberOfJumpsLeft <= 0)
            {
                canJump = false;
            }
            else if (numberOfJumpsLeft > 0)
            {
                canJump = true;
            }
        }

        public void Fall()
        {
            rigidBody2D.velocity += Time.deltaTime * Physics2D.gravity.y * fallGravityMultiplier * Vector2.up;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //if (collision.collider.gameObject.layer == LayerMask.NameToLayer(R.S.Layer.Floor))
               // isGrounded = true;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * transform.localScale.x  * wallDistance);
            Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
        }
    }
}