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
        [SerializeField] private int nbdeath;
        private Sensor sensor;
        private ISensor<Box> boxSensor;
        private Hands hands;
        private bool isLookingRight;
        private Vitals vitals;
        private bool isCrouched;
        private PlayerMover playerMover;
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
            dispatcher = Finder.Dispatcher;

            hands = GetComponentInChildren<Hands>();
            sensor = GetComponentInChildren<Sensor>();
            vitals = GetComponentInChildren<Vitals>();
            playerMover = GetComponent<PlayerMover>();
            
            isLookingRight = true;
            IsDead = false;
            isCrouched = false;
            size = transform.Find(R.S.GameObject.Collider).GetComponent<BoxCollider2D>().bounds.size.y;
            Debug.Log(size);
            
            boxSensor = sensor.For<Box>();
        }

        //Author : Jeammy Côté
        //Change player direction
        public void FlipPlayer()
        {
            transform.localScale = transform.localScale.x == 1 ? new Vector2(-1, 1) : Vector2.one;
        }
        
        //Author : Sébastien Arsenault
        [ContextMenu("Die")]
        public void Die()
        {
            if (!IsDead)
            {
                IsDead = true;
                dispatcher.DataCollector.NbDeath++;
                nbdeath = dispatcher.DataCollector.NbDeath;
                playerDeathEventChannel.NotifyPlayerDeath();
            }
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
            }
        }
        
        public void ThrowBox(bool crouching)
        {
            if (crouching)
                hands.Drop();
            else
                hands.Throw(isLookingRight);
            
            playerMover.ResetSpeed();
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
        
        //Author : Jeammy Côté
        private void OnDrawGizmos()
        {
            var playerBounds = GetComponentInChildren<Collider2D>().bounds;

            Vector3 bottomLeftPosition = new Vector3(playerBounds.center.x - playerBounds.extents.x,playerBounds.center.y - playerBounds.extents.y);
            Vector3 topRightPosition = new Vector3(playerBounds.center.x + playerBounds.extents.x,playerBounds.center.y + playerBounds.extents.y);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(bottomLeftPosition,topRightPosition);
        }
    }
}