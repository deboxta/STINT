using System;
using System.Collections;
using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    //Author : Anthony Bérubé
    public class PlayerMover : MonoBehaviour
    {
        [Header("Abilities to Activate")]
        [SerializeField] private bool hasBoots;
        
        //Boolean for verification in unity editor of surroundings and state of the player
        [Header("Player States")]
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isTouchingWall;
        [SerializeField] private bool isWallSliding;
        [SerializeField] private bool isWallJumping;
        [SerializeField] private bool canJump;
        [SerializeField] private bool playerCanControlMoves;
        [SerializeField] private int numberOfJumpsLeft;
        
        //serializeFields for optimisation and control over moves of the player
        [Header("Player variables")]
        [SerializeField] private float timeBeforePlayerCanControlMovesWhenWallJumping = 0.10f;
        [SerializeField] private int numberOfJumps = 3;
        [SerializeField] private float wallJumpForce = 5;
        [SerializeField] private float wallSlideSpeed = 2f;
        [SerializeField] private float gravityMultiplier = 5f;
        [SerializeField] private int fallGravityMultiplier = 10;
        [SerializeField] private float xSpeed = 10f;
        [SerializeField] private float yForce = 15f;
        [SerializeField] private float groundCheckRadius = 1.11f; 
        [SerializeField] private float wallDistance = 1.11f;
        [SerializeField] private float movementPenalty = 2;
        
        //Raycasts position for ground and wall
        [Header("Player surroundings")]
        [SerializeField] private Transform groundCheck; 
        [SerializeField] private Transform wallCheck; 
        
        private int layersToJump;
        private Vector2 wallJumpDirection;
        private GamePadState gamePadState;
        private Rigidbody2D rigidBody2D;
        private RaycastHit2D wallHit;
        private PlayerAnimator playerAnimator;
        //private Gravity gravity;
        
        //If player has obtained the capacity of wall jumping by collecting the boots
        public bool HasBoots
        {
            set => hasBoots = value;
        }

        private void Awake()
        {
            wallJumpDirection.Normalize();
            rigidBody2D = GetComponent<Rigidbody2D>();
            //gravity = GameObject.FindWithTag(R.S.Tag.GravityObject).GetComponentInChildren<Gravity>();
            
            //https://answers.unity.com/questions/416919/making-raycast-ignore-multiple-layers.html
            //To add a layer do : LayersToHit = |= (1 << LayerMask.NameToLayer(LayerName));
            //Author : Sébastien Arsenault
            layersToJump = (1 << LayerMask.NameToLayer(R.S.Layer.Floor));
            layersToJump |= (1 << LayerMask.NameToLayer(R.S.Layer.OneWay));
            layersToJump |= (1 << LayerMask.NameToLayer(R.S.Layer.MovablePlatform));

            groundCheck = transform.Find("GroundCheck");
            wallCheck = transform.Find("WallCheck");
            playerCanControlMoves = true;
            playerAnimator = Finder.PlayerAnimator;
        }

        private void FixedUpdate()
        {
            //Author : Jeammy Côté
            CheckIfCanJump();
            CheckIfWallSliding();
            CheckSurroundings();
            
            //Author : Anthony Bérubé
            //Player fall faster for more realistic physic
            if (rigidBody2D.velocity.y < 0)
                rigidBody2D.velocity += Time.fixedDeltaTime * Physics2D.gravity.y * gravityMultiplier * Vector2.up;
        }
        
        //Author : Jeammy Côté
        private void CheckSurroundings()
        {
            CheckIfIsGrounded();
            CheckIfIsTouchingWall();
        }

        private void CheckIfIsGrounded()
        {
            bool wasGrounded = isGrounded;
            isGrounded = false;
            
            Vector3 groundCheckPosition = groundCheck.position;
            isGrounded = Physics2D.OverlapCircle(groundCheckPosition, groundCheckRadius, layersToJump);
            if (isGrounded)
            {
                isWallJumping = false;
                if (!wasGrounded)
                    playerAnimator.OnLanding();
            }
        }

        private void CheckIfIsTouchingWall()
        {
            wallHit = Physics2D.Raycast(
                wallCheck.position, 
                transform.right * transform.localScale.x, 
                wallDistance, 
                layersToJump);
            
            if (wallHit)
            {
                isTouchingWall = true;
                wallJumpDirection = wallHit.normal;
                if (!canJump)
                    playerAnimator.WallJumpWarningAnimation();
            }
            else
            {
                isTouchingWall = false;
            }
        }
        
        //Author : Jeammy Côté
        public void Move(Vector2 direction)
        {
            if ((direction != Vector2.zero || !isWallJumping) && playerCanControlMoves)
            {
                if (!isWallSliding || isWallJumping)
                {
                    //Author : Anthony Bérubé
                    var velocity = rigidBody2D.velocity;
                    //Author : Yannick Cote
                    /*if (gravity != null)
                        velocity.x = direction.x * gravity.CalculateForceToApplyX(direction, xSpeed);
                    else*/
                        velocity.x = direction.x * xSpeed;
                    rigidBody2D.velocity = velocity;
                    
                    //Author : Jeammy Côté
                    if(!isWallSliding)
                        playerAnimator.OnMoving(velocity.x);
                }
            }
            //WallSlide
            if (hasBoots)
                if(isWallSliding && rigidBody2D.velocity.y < -wallSlideSpeed)
                    rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, -wallSlideSpeed);
        }

        //Author : Jeammy Côté
        public void Jump()
        {
            //Normal jump
            if (canJump && !isWallSliding && isGrounded)
                //Author : Anthony Bérubé
            {
                //Author : Yannick Cote
                /*if (gravity != null)
                    rigidBody2D.velocity = new Vector2(x: rigidBody2D.velocity.x, gravity.CalculateForceToApplyY(yForce));
                else*/
                rigidBody2D.velocity = new Vector2(x: rigidBody2D.velocity.x, yForce);
                playerAnimator.OnJumping();
            }
            //Wall jump
            else if (canJump && (isWallSliding || isTouchingWall) && !isGrounded)
            {
                WallJump();
                playerAnimator.OnJumping();
            }
        }
        
        //Author : Jeammy Côté
        private void WallJump()
        {
            if (hasBoots)
            {
                isWallSliding = false;
                isWallJumping = true;
                numberOfJumpsLeft--;
                Vector2 forceToAdd;
                
                if (wallHit)
                {
                    //Add pushing force for wall jump with velocity of the moving wall.
                    rigidBody2D.velocity = Vector2.zero;
                    forceToAdd = new Vector2(wallHit.rigidbody.velocity.x * wallJumpForce * wallJumpDirection.x * xSpeed, wallHit.rigidbody.velocity.x * wallJumpForce * yForce +55);
                    rigidBody2D.AddForce(forceToAdd, ForceMode2D.Impulse);
                }
                
                //Add pushing force for wall jump
                rigidBody2D.velocity = Vector2.zero;
                forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * xSpeed, yForce );
                rigidBody2D.AddForce(forceToAdd, ForceMode2D.Impulse);
                
                Finder.Player.FlipPlayer();
                StartCoroutine(StopPlayerMoves());
            }
        }

        //Author : Jeammy Côté
        private void CheckIfWallSliding()
        {
            if (isTouchingWall && !isGrounded)
            {
                isWallSliding = true;
                
                if (!isWallJumping)
                    playerAnimator.OnLanding();
                
                playerAnimator.OnWallSliding();
            }
            else
            {
                isWallSliding = false;
                playerAnimator.OnStopWallSliding();
            }
        }
        
        //Author : Jeammy Côté
        //Check the number of jumps left
        private void CheckIfCanJump()
        {
            if (isGrounded && !isTouchingWall && !isWallSliding && !isWallJumping)
                ResetNumberOfJumpsLeft();

            if (numberOfJumpsLeft <= 0)
                canJump = false;
            else if (numberOfJumpsLeft > 0)
                canJump = true;
        }
        
        //Author : Jeammy Côté
        public void ResetNumberOfJumpsLeft()
        {
            numberOfJumpsLeft = numberOfJumps;
            if (!isGrounded)
                isWallJumping = true;
        }
        
        //Author : Anthony Bérubé
        public void Fall()
        {
            rigidBody2D.velocity += Time.deltaTime * Physics2D.gravity.y * fallGravityMultiplier * Vector2.up;
        }

        //Author : Anthony Bérubé
        public void Slowed()
        {
            xSpeed /= movementPenalty;
            yForce /= movementPenalty;
        }
        
        //Author : Anthony Bérubé
        public void ResetSpeed()
        {
            xSpeed *= movementPenalty;
            yForce *= movementPenalty;
        }
        
        //Author : Jeammy Côté
        private IEnumerator StopPlayerMoves()
        {
            playerCanControlMoves = false;
            yield return new WaitForSeconds(timeBeforePlayerCanControlMovesWhenWallJumping);
            playerCanControlMoves = true;
        }

#if UNITY_EDITOR
        //Author : Jeammy Côté
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (wallCheck != null && groundCheck != null)
            {
                Gizmos.DrawLine(wallCheck.position, wallCheck.position + wallDistance * transform.localScale.x * transform.right);
                Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
            }
        }
#endif     
    }
}