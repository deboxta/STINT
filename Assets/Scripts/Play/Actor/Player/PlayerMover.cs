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
        //BR : Renommer en "canPlayerMove".
        [SerializeField] private bool playerCanControlMoves;
        
        //serializeFields for optimisation and control over moves of the player
        [Header("Player variables")]
        //BC : Renommage à faire. Semble être relié uniquement au "WallJump".
        [SerializeField] private float timeBeforePlayerCanControlMoves = 0.10f;
        //BR : 3 ?
        [SerializeField] private int numberOfJumps = 3;
        [SerializeField] private int numberOfJumpsLeft;
        [SerializeField] private float wallJumpForce = 5;
        [SerializeField] private float wallSlideSpeed = 2f;
        [SerializeField] private float gravityMultiplier = 5f;
        [SerializeField] private int fallGravityMultiplier = 10;
        //BC : Ces deux attributs devraient être un Vector2. 
        [SerializeField] private float xSpeed = 10f;
        [SerializeField] private float yForce = 15f;
        [SerializeField] private float groundCheckRadius = 1.11f; 
        [SerializeField] private float movementPenalty = 2;
        
        //Raycasts position for ground and wall
        [Header("Player surroundings")]
        [SerializeField] private Transform groundCheck; 
        [SerializeField] private Transform wallCheck; 
        
        private bool canJump;
        private float wallDistance = 1.11f;
        private LayerMask floorLayer;
        private Vector2 wallJumpDirection;
        private GamePadState gamePadState;
        private Rigidbody2D rigidBody2D;
        
        //If player has obtained the capacity of wall jumping by collecting the boots
        public bool HasBoots
        {
            set => hasBoots = value;
        }

        private void Awake()
        {
            wallJumpDirection.Normalize();
            rigidBody2D = GetComponent<Rigidbody2D>();
            floorLayer = LayerMask.GetMask(R.S.Layer.Floor);
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
            
            //Author : Anthony Bérubé
            //PLayer fall faster for more realistic physic
            if (rigidBody2D.velocity.y < 0)
                rigidBody2D.velocity += Time.fixedDeltaTime * Physics2D.gravity.y * gravityMultiplier * Vector2.up;
        }
        
        //Author : Jeammy Côté
        //BC : Découpage déficient. À découper en deux (pour le sol et pour les murs).
        private void CheckSurroundings()
        {
            //TODO : Change with cube raycast
            //BR : Il y a de meilleures façon de faire. Pourquoi pas faire un "Cast" du collider du player vers
            //     le bas et vérifier la "normale" de la colision afin de savoir si ce qui a été touché est un sol ou pas.
            //     Voir "rigidBody2D.Cast()".
            //     Voir https://docs.unity3d.com/ScriptReference/Rigidbody2D.Cast.html
            isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,floorLayer);
            if (isGrounded)
                isWallJumping = false;

            RaycastHit2D hit = Physics2D.Raycast(
                wallCheck.position, 
                transform.right * transform.localScale.x,  //BC : Aucune raison de multiplier une direction.
                wallDistance, 
                floorLayer);
            
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
        
        //Author : Jeammy Côté
        public void Move(Vector2 direction)
        {
            if ((direction != Vector2.zero || isGrounded) && playerCanControlMoves)
            {
                if (!isWallSliding && canJump || isWallJumping)
                {
                    //Author : Anthony Bérubé
                    //BR : Utiliser une méthode d'extension aiderait probablement.
                    var velocity = rigidBody2D.velocity;
                    velocity.x = direction.x * xSpeed;
                    rigidBody2D.velocity = velocity;
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
            if (canJump && !isWallSliding && isGrounded)
                //Author : Anthony Bérubé
                rigidBody2D.velocity = new Vector2(x: rigidBody2D.velocity.x , yForce);
            else if (canJump && (isWallSliding || isTouchingWall) && !isGrounded )
                WallJump();
            else if ((isWallJumping || numberOfJumpsLeft <= 0 && isTouchingWall) && !isGrounded )
                WallHop();
        }
        
        //Author : Jeammy Côté
        private void WallJump()
        {
            if (hasBoots)
            {
                isWallSliding = false;
                isWallJumping = true;
                numberOfJumpsLeft--;
                
                //Add pushing force for wall jump
                rigidBody2D.velocity = Vector2.zero;
                Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * xSpeed, yForce );
                rigidBody2D.AddForce(forceToAdd, ForceMode2D.Impulse);
                
                Finder.Player.FlipPlayer();
                StartCoroutine(StopPlayerMoves());
            }
        }
        
        //Author : Jeammy Côté
        //BR : C'est quoi la différence entre WallJump et WallHop ?
        //     Mettre un commentaire aiderais à voir la différence.
        //     Remarque que c'est symptomatique d'un découpage ou d'un nommage déficient.
        private void WallHop()
        {
            isWallSliding = false;
            isWallJumping = false;
                
            //Add pushing force for wall hop
            rigidBody2D.velocity = Vector2.zero;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x, rigidBody2D.velocity.y);
            rigidBody2D.AddForce(forceToAdd, ForceMode2D.Impulse);
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
            //BR : Est-ce que cela devrait être fait seulement si le joueur n'est pas "grounded" ?
            //     Je crois que cela change rien....
            rigidBody2D.velocity += Time.deltaTime * Physics2D.gravity.y * fallGravityMultiplier * Vector2.up;
        }
        
        //Author : Anthony Bérubé
        public void Slowed()
        {
            //BR : Problème de précision en vue. Utilisez plutôt un multiplicateur de vitesse.
            //     Autrement dit, cette méthode va modifier le multiplicateur de vitesse pour "0.5" par exemple.
            //     Lorsque l'on remet le multiplicateur à 1, c'est comme s'il ne s'était rien passé.
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
        //BC : Private manquant.
        IEnumerator StopPlayerMoves()
        {
            playerCanControlMoves = false;
            yield return new WaitForSeconds(timeBeforePlayerCanControlMoves);
            playerCanControlMoves = true;
        }
        
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
    }
}