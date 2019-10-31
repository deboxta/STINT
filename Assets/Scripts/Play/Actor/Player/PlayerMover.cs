using System;
using Harmony;
using UnityEngine;
using UnityEngine.Serialization;
using XInputDotNetPure;

namespace Game
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isTouchingWall;
        [SerializeField] private bool isWallSliding;
        [SerializeField] private bool isWallJumping;
        [SerializeField] private int numberOfJumps = 3;
        [SerializeField] private int numberOfJumpsLeft;
        [SerializeField] private float wallJumpForce = 5;
        [SerializeField] private float wallSlideSpeed = 2f;
        [SerializeField] private float gravityMultiplier = 5f;
        [SerializeField] private int fallGravityMultiplier = 10;
        [SerializeField] private float xSpeed = 10f;
        [SerializeField] private float yForce = 15f;
        [SerializeField] private float groundCheckRadius = 1.11f; 
        [SerializeField] private Transform groundCheck; 
        [SerializeField] private Transform wallCheck; 
        [SerializeField] private float movementPenalty = 2;
        
        private bool canJump;
        private float wallDistance = 1.11f;
        private LayerMask floorLayer;
        private Vector2 wallJumpDirection;
        private GamePadState gamePadState;
        private Rigidbody2D rigidBody2D;

        private void Awake()
        {
            wallJumpDirection.Normalize();
            rigidBody2D = GetComponent<Rigidbody2D>();
            floorLayer = LayerMask.GetMask(R.S.Layer.Floor);
        }

        private void FixedUpdate()
        {
            CheckIfCanJump();
            CheckIfWallSliding();
            CheckSurroundings();
            if (rigidBody2D.velocity.y < 0)
                rigidBody2D.velocity += Time.deltaTime * Physics2D.gravity.y * gravityMultiplier * Vector2.up;
        }

        private void CheckSurroundings()
        {
            //TODO : Change with cube raycast
            isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,floorLayer);
            if (isGrounded)
            {
                isWallJumping = false;
            }
            
            RaycastHit2D hit = Physics2D.Raycast(
                wallCheck.position, 
                Vector2.right * transform.localScale.x, 
                wallDistance, 
                floorLayer);
            
            if (hit)
            {
                isTouchingWall = true;
                
                //change player direction left or right
                transform.localScale = transform.localScale.x == 1 ? new Vector2(-1, 1) : Vector2.one;
                
                wallJumpDirection = hit.normal;
            }
            else
            {
                isTouchingWall = false;
            }
        }

        public void Move(Vector2 direction)
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);
            
            if (!isWallSliding && !isWallJumping && numberOfJumpsLeft > 0)
            {
                var velocity = rigidBody2D.velocity;
                velocity.x = direction.x * xSpeed;
                rigidBody2D.velocity = velocity;
            }

            if ((isWallJumping || numberOfJumpsLeft <= 0) && 
                (Input.GetKeyDown(KeyCode.Space) || gamePadState.Buttons.A == ButtonState.Pressed) && !isGrounded)
            {
                if (wallJumpDirection == new Vector2(transform.localScale.x,transform.localScale.y))
                {
                    rigidBody2D.velocity = new Vector2(xSpeed * transform.localScale.x * -1 ,yForce);
                }
            }
            
            if (isWallSliding)
            {
                if(rigidBody2D.velocity.y < -wallSlideSpeed)
                {
                    rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, -wallSlideSpeed);
                }
            }
        }

        public void Jump()
        {
            if (canJump && !isWallSliding && isGrounded)
            {
                rigidBody2D.velocity = new Vector2(x: rigidBody2D.velocity.x , yForce);
            }
            else if (canJump && (isWallSliding || isTouchingWall) && !isGrounded)
            {
                isWallSliding = false;
                isWallJumping = true;
                numberOfJumpsLeft--;
                
                //Add pushing force for wall jump
                rigidBody2D.velocity = Vector2.zero;
                Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * xSpeed, yForce );
                rigidBody2D.AddForce(forceToAdd, ForceMode2D.Impulse);
            }
            else if ((isWallJumping || numberOfJumpsLeft <= 0 && isTouchingWall) && !isGrounded) //Wall hop
            {
                isWallSliding = false;
                isWallJumping = false;
                
                //Add pushing force for wall hop
                rigidBody2D.velocity = Vector2.zero;
                Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x, rigidBody2D.velocity.y );
                rigidBody2D.AddForce(forceToAdd, ForceMode2D.Impulse);
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
            if (isGrounded && !isTouchingWall && !isWallSliding)
            {
                ResetNumberOfJumpsLeft();
            }

            if (numberOfJumpsLeft <= 0)
            {
                canJump = false;
                isWallJumping = false;
            }
            else if (numberOfJumpsLeft > 0)
            {
                canJump = true;
            }
        }

        public void ResetNumberOfJumpsLeft()
        {
            numberOfJumpsLeft = numberOfJumps;
            isWallJumping = true;
        }

        public void Fall()
        {
            rigidBody2D.velocity += Time.deltaTime * Physics2D.gravity.y * fallGravityMultiplier * Vector2.up;
        }
        
        public void Slowed()
        {
            xSpeed /= movementPenalty;
            yForce /= movementPenalty;
        }
        
        public void ResetSpeed()
        {
            xSpeed *= movementPenalty;
            yForce *= movementPenalty;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * transform.localScale.x  * wallDistance);
            Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
        }
    }
}