using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Anthony Bérubé
    
    [Findable(R.S.Tag.Player)]
    [RequireComponent(typeof(PlayerMover), typeof(PlayerInput))]
    public class Player : MonoBehaviour , IPowerUpCollector
    {
        private PlayerDeathEventChannel playerDeathEventChannel;
        private SavedSceneLoadedEventChannel savedSceneLoadedEventChannel;
        private Sensor sensor;
        private ISensor<Box> boxSensor;
        private Hands hands;
        private Vitals vitals;
        private PlayerInput playerInput;
        private BoxCollider2D boxCollider2D;
        private Rigidbody2D rigidBody2D;
        private Dispatcher dispatcher;

        public PlayerMover PlayerMover { get; set; }
        public Hands Hands => hands;
        public Vitals Vitals => vitals;
        public bool IsDead { get; set; }

        public bool IsLookingRight
        {
            get => transform.localScale.x >= 0;
            set => transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * (value ? 1 : -1), transform.localScale.y);
        }
        
        public bool IsHoldingBox => hands.IsHoldingBox;
        
        private bool IsSensingBox => boxSensor.SensedObjects.Count > 0;

        private Box SensedBox => boxSensor.SensedObjects[0];

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
            PlayerMover = GetComponent<PlayerMover>();
            playerInput = GetComponent<PlayerInput>();
            boxCollider2D = transform.Find(R.S.GameObject.Collider).GetComponent<BoxCollider2D>();
            rigidBody2D = GetComponent<Rigidbody2D>();

            IsLookingRight = true;
            IsDead = false;
            size = boxCollider2D.bounds.size.y;
            
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
            IsLookingRight = !IsLookingRight;
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
                playerDeathEventChannel.NotifyPlayerDeath();
            }
        }

        //Author : Sébastien Arsenault
        private void DeactivateComponentsWhenDead()
        {
            PlayerMover.enabled = false;
            playerInput.enabled = false;
            boxCollider2D.enabled = false;
            rigidBody2D.isKinematic = true;
            rigidBody2D.velocity = Vector2.zero;
        }

        //Grabs the box
        public void GrabBox()
        {
            //If the player isn't holding a box and if there is a box in his sensor
            if (!IsHoldingBox && IsSensingBox)
            {
                //Grabs the box
                hands.Grab(SensedBox);
                PlayerMover.Slowed();
                Finder.PlayerAnimator.OnGrabBox();
            }
        }

        public void ThrowBox()
        {
            hands.Throw(IsLookingRight);
            
            PlayerMover.ResetSpeed();
            Finder.PlayerAnimator.OnBoxThrow();
        }
        
        public void DropBox()
        {
            hands.Drop();
            
            PlayerMover.ResetSpeed();
            Finder.PlayerAnimator.OnBoxThrow();
        }
        
        //Author : Jeammy Côté
        public void CollectPowerUp()
        {
            PlayerMover.ResetNumberOfJumpsLeft();
        }
        
        //Author : Jeammy Côté
        public void CollectBoots()
        {
            PlayerMover.HasBoots = true;
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