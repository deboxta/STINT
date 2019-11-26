using System;
using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    //Author : Anthony Bérubé
    
    [Findable(R.S.Tag.Player)]
    [RequireComponent(typeof(PlayerMover), typeof(PlayerInput))]
    public class Player : MonoBehaviour , IPowerUpCollector
    {
        private PlayerDeathEventChannel playerDeathEventChannel;
        private SavedSceneLoadedEventChannel savedSceneLoadedEventChannel;
        [SerializeField] private int nbDeath;
        private Sensor sensor;
        private ISensor<Box> boxSensor;
        private Hands hands;
        private bool isLookingRight;
        private Vitals vitals;
        private bool isCrouched;
        private PlayerMover playerMover;
        private PlayerInput playerInput;
        private BoxCollider2D boxCollider2D;
        private Rigidbody2D rigidBody2D;
        private Dispatcher dispatcher;

        public Hands Hands => hands;
        public Vitals Vitals => vitals;
        public bool IsDead { get; set; }

        public bool IsLookingRight 
        { 
            set => isLookingRight = value;
        }

        private float size;
        public float Size => size;
        

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            savedSceneLoadedEventChannel = Finder.SavedSceneLoadedEventChannel;
            dispatcher = Finder.Dispatcher;

            hands = GetComponentInChildren<Hands>();
            sensor = GetComponentInChildren<Sensor>();
            vitals = GetComponentInChildren<Vitals>();
            playerMover = GetComponent<PlayerMover>();
            playerInput = GetComponent<PlayerInput>();
            boxCollider2D = transform.Find(R.S.GameObject.Collider).GetComponent<BoxCollider2D>();
            rigidBody2D = GetComponent<Rigidbody2D>();

            isLookingRight = true;
            IsDead = false;
            isCrouched = false;
            //TODO CHANGE THIS SEB NOTE
            size = transform.Find(R.S.GameObject.Collider).GetComponent<BoxCollider2D>().bounds.size.y;
            
            boxSensor = sensor.For<Box>();
        }

        private void OnEnable()
        {
            savedSceneLoadedEventChannel.OnSavedSceneLoaded += SavedSceneLoaded;
        }

        private void OnDisable()
        {
            savedSceneLoadedEventChannel.OnSavedSceneLoaded -= SavedSceneLoaded;
        }

        private void SavedSceneLoaded()
        {
            StartCoroutine(ChangePosition());
        }

        private IEnumerator ChangePosition()
        {
            yield return transform.position = new Vector3(dispatcher.DataCollector.PositionX,dispatcher.DataCollector.PositionY);
        }

        //Author : Jeammy Côté
        //Change player direction
        public void FlipPlayer()
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        
        //Author : Sébastien Arsenault
        [ContextMenu("Die")]
        public void Die()
        {
            if (!IsDead)
            {
                IsDead = true;
                DeactivateComponentsWhenDead();
                dispatcher.DataCollector.NbDeath++;
                nbDeath = dispatcher.DataCollector.NbDeath;
                playerDeathEventChannel.NotifyPlayerDeath();
            }
        }

        //Author : Sébastien Arsenault
        private void DeactivateComponentsWhenDead()
        {
            playerMover.enabled = false;
            playerInput.enabled = false;
            boxCollider2D.enabled = false;
            rigidBody2D.isKinematic = true;
            rigidBody2D.velocity = Vector2.zero;
        }

        //TODO : LOOK FOR THE NEAREST BOX IN CASE THERE'S TWO AND THE DIRECTION
        //Grabs the box
        public void GrabBox()
        {
            //If the player isn't holding a box and if there is a box in his sensor
            if (!hands.IsHoldingBox && boxSensor.SensedObjects.Count > 0)
            {
                //Grabs the box
                hands.Grab(boxSensor.SensedObjects[0]);
                playerMover.Slowed();
                Finder.PlayerAnimator.OnGrabBox();
            }
        }

        public void ThrowBox(bool crouching)
        {
            if (crouching)
                hands.Drop();
            else
                hands.Throw(isLookingRight);
            
            playerMover.ResetSpeed();
            Finder.PlayerAnimator.OnBoxThrow();
        }
        
        //Author : Jeammy Côté
        public void CollectPowerUp()
        {
            playerMover.ResetNumberOfJumpsLeft();
        }
        
        //Author : Jeammy Côté
        public void CollectBoots()
        {
            playerMover.HasBoots = true;
        }
#if UNITY_EDITOR
        //Author : Jeammy Côté
        private void OnDrawGizmos()
        {
            var playerBounds = GetComponentInChildren<Collider2D>().bounds;

            Vector3 bottomLeftPosition = new Vector3(playerBounds.center.x - playerBounds.extents.x,playerBounds.center.y - playerBounds.extents.y);
            Vector3 topRightPosition = new Vector3(playerBounds.center.x + playerBounds.extents.x,playerBounds.center.y + playerBounds.extents.y);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(bottomLeftPosition,topRightPosition);
        }
#endif
    }
}