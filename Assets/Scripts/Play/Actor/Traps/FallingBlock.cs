using System;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class FallingBlock : MonoBehaviour, IFreezable
    {
        [SerializeField] private float fallSpeed = 0.2f;
        [SerializeField] private float startingDistance = 10f;

        private new Rigidbody2D rigidbody2D;
        private Sensor sensor;
        private DeadlyTrap deadlyTrap;
        private ISensor<Player> playerSensor;
        private TimeFreezeEventChannel timeFreezeEventChannel;
        private Vector2 velocityBeforeFreeze;
        private bool wasKinematicBeforeFreeze;
        private bool isFalling;
        private bool isGrounded;
        private float playerSize;

        public bool Frozen => Finder.TimeFreezeController.IsFrozen;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            playerSensor = GetComponentInChildren<Sensor>().For<Player>();
            sensor = GetComponentInChildren<Sensor>();
            sensor.transform.position = new Vector3(sensor.transform.position.x,
                sensor.transform.position.y - sensor.YSize / 2, 0);
            deadlyTrap = GetComponentInChildren<DeadlyTrap>();
            timeFreezeEventChannel = Finder.TimeFreezeEventChannel;
            velocityBeforeFreeze = Vector2.zero;
            wasKinematicBeforeFreeze = true;
            transform.position = new Vector3(transform.position.x, transform.position.y + startingDistance,
                transform.position.z);
            isFalling = false;
            isGrounded = false;
            playerSize = 0;
        }

        private void FixedUpdate()
        {
            if (!isGrounded && isFalling && !Frozen)
            {
                Fall();
            }

            if (playerSensor.SensedObjects.Count > 0)
            {
                if (startingDistance < playerSize)
                    playerSensor.SensedObjects[0].Die();
            }
        }

        private void OnEnable()
        {
            timeFreezeEventChannel.OnTimeFreezeStateChanged += OnTimeFreezeStateChanged;
            playerSensor.OnSensedObject += OnPlayerSensed;
        }

        private void OnDisable()
        {
            timeFreezeEventChannel.OnTimeFreezeStateChanged -= OnTimeFreezeStateChanged;
            playerSensor.OnSensedObject -= OnPlayerSensed;
        }

        private void OnTimeFreezeStateChanged()
        {
            if (Frozen)
            {
                velocityBeforeFreeze = rigidbody2D.velocity;
                wasKinematicBeforeFreeze = rigidbody2D.isKinematic;
            }
            else
            {
                rigidbody2D.velocity = velocityBeforeFreeze;
                rigidbody2D.isKinematic = wasKinematicBeforeFreeze;
                if (!rigidbody2D.isKinematic)
                    deadlyTrap.enabled = true;
            }
        }

        private void Fall()
        {
            transform.position -= new Vector3(0, fallSpeed, 0);
            if (Math.Abs(startingDistance) > 0.2f)
                startingDistance -= fallSpeed;
            else
                isGrounded = true;
        }

        private void OnPlayerSensed(Player player)
        {
            playerSize = player.Size;
            if (!Frozen)
                isFalling = true;
        }
    }
}