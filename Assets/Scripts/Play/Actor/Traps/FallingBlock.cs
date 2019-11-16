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

        private BoxCollider2D sensorBoxCollider2D;
        private BoxCollider2D blockBoxCollider2D;
        private Sensor sensor;
        private ISensor<Player> playerSensor;
        private bool wasKinematicBeforeFreeze;
        private bool isFalling;
        private bool isGrounded;
        private float playerSize;

        public bool IsFrozen => Finder.TimeFreezeController.IsFrozen;

        private void Awake()
        {
            blockBoxCollider2D = transform.Find(R.S.GameObject.Collider).GetComponent<BoxCollider2D>();
            playerSensor = GetComponentInChildren<Sensor>().For<Player>();
            sensor = GetComponentInChildren<Sensor>();
            sensor.transform.position = new Vector3(sensor.transform.position.x,
                -startingDistance / 2, 0);
            sensorBoxCollider2D = transform.Find(R.S.GameObject.Sensor).GetComponent<BoxCollider2D>();
            sensorBoxCollider2D.size = new Vector2(blockBoxCollider2D.size.x, startingDistance);
            transform.position = new Vector3(transform.position.x, transform.position.y + startingDistance,
                transform.position.z);
            isFalling = false;
            isGrounded = false;
            playerSize = 0;
        }

        private void FixedUpdate()
        {
            if (!isGrounded && isFalling && !IsFrozen)
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
            playerSensor.OnSensedObject += OnPlayerSensed;
        }

        private void OnDisable()
        {
            playerSensor.OnSensedObject -= OnPlayerSensed;
        }

        private void Fall()
        {
            transform.position -= new Vector3(0, fallSpeed, 0);
            sensorBoxCollider2D.size = new Vector2(sensorBoxCollider2D.size.x, sensorBoxCollider2D.size.y - fallSpeed);
            sensor.transform.position = new Vector3(sensor.transform.position.x, sensor.transform.position.y + fallSpeed / 2, sensor.transform.position.z);
            if (Math.Abs(startingDistance) > 0.2f)
            {
                startingDistance -= fallSpeed;
            }
            else
                isGrounded = true;
        }

        private void OnPlayerSensed(Player player)
        {
            playerSize = player.Size;
            if (!IsFrozen)
                isFalling = true;
        }
    }
}