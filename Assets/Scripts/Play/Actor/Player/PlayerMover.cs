using System;
using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.Serialization;
using XInputDotNetPure;

namespace Game
{
    //Author : Anthony Bérubé
    public class PlayerMover : MonoBehaviour
    {
        public event PlayerJumpEventHandler OnPlayerJump;
        
        [Header("Abilities to Activate")]
        [SerializeField] private bool hasBoots;
        
        //Boolean for verification in unity editor of surroundings and state of the player
        [Header("Player States")]
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isTouchingWall;
        [SerializeField] private bool isWallSliding;
        [SerializeField] private bool isWallJumping;
        [SerializeField] private bool canJump;
        [SerializeField] private bool canPlayerMove;
        [SerializeField] private int numberOfJumpsLeft;
        
        //serializeFields for optimisation and control over moves of the player
        [Header("Player variables")]
        [SerializeField] private float timeBeforePlayerCanControlMovesWhenWallJumping = 0.10f;
        [SerializeField] private int numberOfJumps = 3;
        [SerializeField] private float wallJumpForce = 5;
        [SerializeField] private float wallSlideSpeed = 2f;
        [SerializeField] private float gravityMultiplier = 5f;
        [SerializeField] private int fallGravityMultiplier = 10;
        [SerializeField] private Vector2 playerMovementForce = new Vector2(15f, 17f);
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
        private RaycastHit2D wallHit;
        private Gravity gravity;
        private bool isGravityNotNull;
        private PlayerAnimator playerAnimator;
        
        private Rigidbody2D rigidBody2D;

        public float? YVelocityToLerp { get; set; }
        public float YVelocityLerpTValue { get; set; }

        //If player has obtained the capacity of wall jumping by collecting the boots
        public bool HasBoots
        {
            set => hasBoots = value;
        }

        private void Awake()
        {
            wallJumpDirection.Normalize();
            rigidBody2D = GetComponent<Rigidbody2D>();

            //by Yannick Cote
            GameObject gravityObject = GameObject.FindWithTag(R.S.Tag.GravityObject);
            if (gravityObject != null)
                gravity = gravityObject.GetComponentInChildren<Gravity>();
            isGravityNotNull = gravity != null;

            //https://answers.unity.com/questions/416919/making-raycast-ignore-multiple-layers.html
            //To add a layer do : LayersToHit = |= (1 << LayerMask.NameToLayer(LayerName));
            //Author : Sébastien Arsenault
            layersToJump = (1 << LayerMask.NameToLayer(R.S.Layer.Floor));
            layersToJump |= (1 << LayerMask.NameToLayer(R.S.Layer.OneWay));
            layersToJump |= (1 << LayerMask.NameToLayer(R.S.Layer.MovablePlatform));

            groundCheck = transform.Find("GroundCheck");
            wallCheck = transform.Find("WallCheck");
            canPlayerMove = true;
            playerAnimator = Finder.PlayerAnimator;
        }

        private void FixedUpdate()
        {
            //Author : Jeammy Côté
            CheckIfCanJump();
            CheckIfWallSliding();
            CheckSurroundings();

            float newYVelocity = rigidBody2D.velocity.y;
            //Author : Anthony Bérubé
            //Player fall faster for more realistic physics
            if (rigidBody2D.velocity.y < 0)
//                RigidBody2D.velocity += Time.fixedDeltaTime * Physics2D.gravity.y * gravityMultiplier * Vector2.up;
            {
                newYVelocity += Time.fixedDeltaTime * Physics2D.gravity.y * gravityMultiplier;
            }

            if (YVelocityToLerp != null)
            {
                newYVelocity = Mathf.Lerp(newYVelocity, (float) YVelocityToLerp, YVelocityLerpTValue);
            }
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, newYVelocity);
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
                {
                    playerAnimator.OnLanding();
                    //by Yannick Cote
                    if (isGravityNotNull && gravity.isActiveAndEnabled)
                        gravity.DeactivatePointEffector(false);
                }
            }
        }

        private void CheckIfIsTouchingWall()
        {
            var transform1 = transform;
            wallHit = Physics2D.Raycast(
                wallCheck.position, 
                transform1.right * transform1.localScale.x, 
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
            if ((direction != Vector2.zero || !isWallJumping) && canPlayerMove)
            {
                if (!isWallSliding || isWallJumping)
                {
                    //Author : Anthony Bérubé
                    var velocity = rigidBody2D.velocity;
                    //Author : Yannick Cote
                    if (isGravityNotNull && gravity.isActiveAndEnabled)
                        velocity.x = direction.x * gravity.CalculateForceToApplyX(direction, playerMovementForce.x);
                    else
                        velocity.x = direction.x * playerMovementForce.x;
                    rigidBody2D.velocity = velocity;
                    
                    //Author : Jeammy Côté
                    if(!isWallSliding)
                        playerAnimator.OnMoving(velocity.x);
                }
            }

            //WallSlide
            if (hasBoots)
                if (isWallSliding && rigidBody2D.velocity.y < -wallSlideSpeed)
                    rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, -wallSlideSpeed);
        }

        //Author : Jeammy Côté
        public void Jump()
        {
            //By Yannick Cote
            if (isGravityNotNull && gravity.isActiveAndEnabled)
                gravity.DeactivatePointEffector(true);
            //Normal jump
            if (canJump && !isWallSliding && isGrounded)
                //Author : Anthony Bérubé
            {
                //Author : Yannick Cote
                if (isGravityNotNull && gravity.isActiveAndEnabled)
                    rigidBody2D.velocity = new Vector2(x: rigidBody2D.velocity.x, gravity.CalculateForceToApplyY(playerMovementForce.y));
                else
                    rigidBody2D.velocity = new Vector2(x: rigidBody2D.velocity.x, playerMovementForce.y);
                NotifyPlayerJump();
            }
            //Wall jump
            else if (canJump && (isWallSliding || isTouchingWall) && !isGrounded)
            {
                WallJump();
                NotifyPlayerJump();
            }
        }

        //Author : Jeammy Côté
        private void WallJump()
        {
            //By Yannick Cote
            if (isGravityNotNull && gravity.isActiveAndEnabled)
                gravity.DeactivatePointEffector(true);
            if (hasBoots)
            {
                isWallSliding = false;
                isWallJumping = true;
                numberOfJumpsLeft--;
                Vector2 forceToAdd;

                if (wallHit)
                {
                    //Add pushing force for wall jump with velocity of the wall.
                    rigidBody2D.velocity = Vector2.zero;
                    var velocity = wallHit.rigidbody.velocity;
                    forceToAdd = new Vector2(velocity.x * wallJumpForce * wallJumpDirection.x * playerMovementForce.x, velocity.x * wallJumpForce * playerMovementForce.y);
                    rigidBody2D.AddForce(forceToAdd, ForceMode2D.Impulse);
                }

                //Add pushing force for wall jump
                rigidBody2D.velocity = Vector2.zero;
                forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * playerMovementForce.x, playerMovementForce.y );
                rigidBody2D.AddForce(forceToAdd, ForceMode2D.Impulse);

                
                NotifyPlayerJump();
                
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
            playerMovementForce.x /= movementPenalty;
            playerMovementForce.y /= movementPenalty;
        }

        //Author : Anthony Bérubé
        public void ResetSpeed()
        {
            playerMovementForce.x *= movementPenalty;
            playerMovementForce.y *= movementPenalty;
        }

        //Author : Jeammy Côté
        private IEnumerator StopPlayerMoves()
        {
            canPlayerMove = false;
            yield return new WaitForSeconds(timeBeforePlayerCanControlMovesWhenWallJumping);
            canPlayerMove = true;
        }
        
        private void NotifyPlayerJump() 
        { 
            if (OnPlayerJump != null) OnPlayerJump();
        }

#if UNITY_EDITOR
        //Author : Jeammy Côté
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (wallCheck != null && groundCheck != null)
            {
                var position = wallCheck.position;
                var transform1 = transform;
                Gizmos.DrawLine(position, position + wallDistance * transform1.localScale.x * transform1.right);
                Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
            }
        }
#endif   
        
        public delegate void PlayerJumpEventHandler();
    }
}