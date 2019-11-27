using System.Collections;
using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    //Author : Anthony Bérubé
    public class PlayerMover : MonoBehaviour
    {
        [Header("Abilities to Activate")] [SerializeField]
        private bool hasBoots;

        //Boolean for verification in unity editor of surroundings and state of the player
        [Header("Player States")] [SerializeField]
        private bool isGrounded;

        [SerializeField] private bool isTouchingWall;
        [SerializeField] private bool isWallSliding;
        [SerializeField] private bool isWallJumping;
        [SerializeField] private bool canJump;
        [SerializeField] private bool playerCanControlMoves;
        [SerializeField] private int numberOfJumpsLeft;

        //serializeFields for optimisation and control over moves of the player
        [Header("Player variables")] [SerializeField]
        private float timeBeforePlayerCanControlMoves = 0.10f;

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
        [Header("Player surroundings")] [SerializeField]
        private Transform groundCheck;

        [SerializeField] private Transform wallCheck;

        private int layersToJump;
        private Vector2 wallJumpDirection;
        private GamePadState gamePadState;
        private RaycastHit2D wallHit;

        public Rigidbody2D RigidBody2D { get; set; }

//        public float? ModifiedYVelocity { get; set; }
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
            RigidBody2D = GetComponent<Rigidbody2D>();

            //https://answers.unity.com/questions/416919/making-raycast-ignore-multiple-layers.html
            //To add a layer do : LayersToHit = |= (1 << LayerMask.NameToLayer(LayerName));
            //Author : Sébastien Arsenault
            layersToJump = (1 << LayerMask.NameToLayer(R.S.Layer.Floor));
            layersToJump |= (1 << LayerMask.NameToLayer(R.S.Layer.OneWay));
            layersToJump |= (1 << LayerMask.NameToLayer(R.S.Layer.MovablePlatform));

            groundCheck = transform.Find("GroundCheck");
            wallCheck = transform.Find("WallCheck");
            playerCanControlMoves = true;
        }

        private void FixedUpdate()
        {
            //Author : Jeammy Côté
            CheckIfCanJump();
            CheckIfWallSliding();
            CheckSurroundings();

            float newYVelocity = RigidBody2D.velocity.y;
            //Author : Anthony Bérubé
            //Player fall faster for more realistic physics
            if (RigidBody2D.velocity.y < 0)
//                RigidBody2D.velocity += Time.fixedDeltaTime * Physics2D.gravity.y * gravityMultiplier * Vector2.up;
            {
                newYVelocity += Time.fixedDeltaTime * Physics2D.gravity.y * gravityMultiplier;
            }

            if (YVelocityToLerp != null)
            {
                newYVelocity = Mathf.Lerp(newYVelocity, (float) YVelocityToLerp, YVelocityLerpTValue);
            }
            RigidBody2D.velocity = new Vector2(RigidBody2D.velocity.x, newYVelocity);
        }

        //Author : Jeammy Côté
        private void CheckSurroundings()
        {
            //TODO : Change with cube raycast
            var position = groundCheck.position;

            isGrounded = Physics2D.OverlapCircle(position, groundCheckRadius, layersToJump);
            if (isGrounded)
                isWallJumping = false;

            wallHit = Physics2D.Raycast(
                wallCheck.position,
                transform.right * transform.localScale.x,
                wallDistance,
                layersToJump);

            if (wallHit)
            {
                isTouchingWall = true;
                wallJumpDirection = wallHit.normal;
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
                    var velocity = RigidBody2D.velocity;
                    velocity.x = direction.x * xSpeed;
                    RigidBody2D.velocity = velocity;
                }
            }

            //WallSlide
            if (hasBoots)
                if (isWallSliding && RigidBody2D.velocity.y < -wallSlideSpeed)
                    RigidBody2D.velocity = new Vector2(RigidBody2D.velocity.x, -wallSlideSpeed);
        }

        //Author : Jeammy Côté
        public void Jump()
        {
            if (canJump && !isWallSliding && isGrounded)
                //Author : Anthony Bérubé
                RigidBody2D.velocity = new Vector2(x: RigidBody2D.velocity.x, yForce);
            else if (canJump && (isWallSliding || isTouchingWall) && !isGrounded)
                WallJump();
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
                    //Add pushing force for wall jump with velocity of the wall.
                    RigidBody2D.velocity = Vector2.zero;
                    forceToAdd =
                        new Vector2(wallHit.rigidbody.velocity.x * wallJumpForce * wallJumpDirection.x * xSpeed,
                                    yForce);
                    RigidBody2D.AddForce(forceToAdd, ForceMode2D.Impulse);
                }

                //Add pushing force for wall jump
                RigidBody2D.velocity = Vector2.zero;
                forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * xSpeed, yForce);
                RigidBody2D.AddForce(forceToAdd, ForceMode2D.Impulse);

                Finder.Player.FlipPlayer();
                StartCoroutine(StopPlayerMoves());
            }
        }

        //Author : Jeammy Côté
        private void CheckIfWallSliding()
        {
            if (isTouchingWall && !isGrounded)
                isWallSliding = true;
            else
                isWallSliding = false;
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
            RigidBody2D.velocity += Time.deltaTime * Physics2D.gravity.y * fallGravityMultiplier * Vector2.up;
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
            yield return new WaitForSeconds(timeBeforePlayerCanControlMoves);
            playerCanControlMoves = true;
        }

#if UNITY_EDITOR
        //Author : Jeammy Côté
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (wallCheck != null && groundCheck != null)
            {
                Gizmos.DrawLine(wallCheck.position,
                                wallCheck.position + wallDistance * transform.localScale.x * transform.right);
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }
#endif
    }
}