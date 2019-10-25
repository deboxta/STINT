using UnityEngine;

namespace Game
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float gravityMultiplier = 5f;
        [SerializeField] private int fallGravityMultiplier = 10;
        [SerializeField] private bool canJump;
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isTouchingWall;
        [SerializeField] private bool isWallSliding;
        [SerializeField] private int numberOfJumps = 3;
        [SerializeField] private int numberOfJumpsLeft;
        [SerializeField] private float wallJumpForce = 5;
        [SerializeField] private float wallDistance = 1f;
        [SerializeField] private float groundCheckRadius; 
        [SerializeField] private Transform groundCheck; 
        [SerializeField] private Transform wallCheck; 
        [SerializeField] private LayerMask floorLayer;
        [SerializeField] private float wallSlideSpeed;
        [SerializeField] private Vector2 wallJumpDirection;
        
        [SerializeField] private float xSpeed = 10f;
        [SerializeField] private float yForce = 15f;
        [SerializeField] private float movementPenality = 2;

        private Rigidbody2D rigidBody2D;

        public bool IsGrounded => isGrounded;
        public bool IsTouchingWall => isTouchingWall;

        private void Awake()
        {
            wallJumpDirection.Normalize();
            rigidBody2D = GetComponent<Rigidbody2D>();
            wallSlideSpeed = 2f;
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
            RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, Vector2.right * transform.localScale.x, wallDistance, floorLayer);
            if (hit)
            {
                isTouchingWall = true;
                wallJumpDirection = hit.normal;
            }
            else
            {
                isTouchingWall = false;
            }
        }

        public void Move(Vector2 direction)
        {
            var velocity = rigidBody2D.velocity;
            velocity.x = direction.x * xSpeed;
            rigidBody2D.velocity = velocity;
            
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
                rigidBody2D.velocity = new Vector2(x: rigidBody2D.velocity.x, yForce);
                numberOfJumpsLeft--;
            }
            else if (canJump && (isWallSliding || isTouchingWall) && !isGrounded)
            {
                isWallSliding = false;
                numberOfJumpsLeft--;
                Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x , wallJumpForce );
                rigidBody2D.AddForce(forceToAdd, ForceMode2D.Impulse);
            }
        }

        private void CheckIfWallSliding()
        {
            if (isTouchingWall && !isGrounded)
            {
                isWallSliding = true;
                transform.localScale = transform.localScale.x == 1 ? new Vector2(-1, 1) : Vector2.one;
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * transform.localScale.x  * wallDistance);
            Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
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