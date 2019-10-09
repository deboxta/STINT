using System;
using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    [Findable(R.S.Tag.Player)]
    [RequireComponent(typeof(PlayerMover))]
    
    public class Player : MonoBehaviour
    {
        private const int MAX_MENTAL_HEALTH = 100;
        
        private PlayerDeathEventChannel playerDeathEventChannel;
        private Sensor sensor;
        private int mentalHealth;
        private ISensor<Box> boxSensor;
        
        private Hands hands;
        public Hands Hands => hands;

        private bool isLookingRight;
        public bool IsLookingRight 
        { 
            set => isLookingRight = value;
        }


        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;

            hands = GetComponentInChildren<Hands>();
            sensor = GetComponentInChildren<Sensor>();
            
            mentalHealth = MAX_MENTAL_HEALTH;

            isLookingRight = true;
            
            boxSensor = sensor.For<Box>();
        }

        private void Update()
        {
            if (mentalHealth <= 0)
            {
                Die();
            }
        }

        private void FixedUpdate()
        {
            FlipPlayer();
        }

        //Turn the player in the right direction (and the box in his hand technicly)
        private void FlipPlayer()
        {
            if (!isLookingRight)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }

        public void Die()
        {
            playerDeathEventChannel.NotifyPlayerDeath();
        }

        //TODO : LOOK FOR THE NEAREST BOX IN CASE THERE'S TWO
        //Grabs the box
        public void GrabBox()
        {
            //If the player isn't holding a box and if there is a box in his sensor
            if (!hands.IsHoldingBox && boxSensor.SensedObjects.Count > 0)
            {
                //Grabs the box
                hands.Grab(boxSensor.SensedObjects[0]);
            }
        }
        
        public void ThrowBox()
        {
            hands.Throw(isLookingRight);
        }
    }
}