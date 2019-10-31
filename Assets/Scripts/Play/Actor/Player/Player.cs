using Harmony;
using TreeEditor;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.Player)]
    [RequireComponent(typeof(PlayerMover), typeof(PlayerInput))]
    public class Player : MonoBehaviour , IPowerUpCollector
    {
        private PlayerDeathEventChannel playerDeathEventChannel;
        private Sensor sensor;
        private ISensor<Box> boxSensor;
        private Hands hands;
        private bool isLookingRight;
        private Vitals vitals;
        private bool isCrouched;
        private PlayerMover playerMover;

        public Hands Hands => hands;
        public Vitals Vitals
        {
            get => vitals;
        }
        public bool IsDead { get; set; }

        public bool IsLookingRight 
        { 
            set => isLookingRight = value;
        }

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;

            hands = GetComponentInChildren<Hands>();
            sensor = GetComponentInChildren<Sensor>();
            vitals = GetComponentInChildren<Vitals>();
            playerMover = GetComponent<PlayerMover>();
            
            isLookingRight = true;
            IsDead = false;
            isCrouched = false;
            
            boxSensor = sensor.For<Box>();
        }

        //Change player direction
        public void FlipPlayer()
        {
            transform.localScale = transform.localScale.x == 1 ? new Vector2(-1, 1) : Vector2.one;
        }
        
        //Change player direction to the right
        public void FlipPlayerRight()
        {
            if (!isLookingRight)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }

        [ContextMenu("Die")]
        public void Die()
        {
            if (!IsDead)
            {
                IsDead = true;
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

        public void CollectPowerUp()
        {
            playerMover.ResetNumberOfJumpsLeft();
        }
    }
}