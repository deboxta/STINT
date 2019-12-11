using System;
using Harmony;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    //Author : Sébastien Arsenault
    public class FallingBlock : MonoBehaviour, IFreezable
    {
        [SerializeField] private float fallSpeed = 0.2f;

        [FormerlySerializedAs("startingDistance")] [SerializeField]
        private float startingDistanceFromGround = 10f;

        [Range(0, 1)] [SerializeField] private float sensorXMargin = 0.1f;

        private BoxCollider2D sensorBoxCollider2D;
        private BoxCollider2D blockBoxCollider2D;
        private Sensor sensor;
        private ISensor<Player> playerSensor;
        private bool isFalling;
        private float playerSize;

        private static bool IsFrozen => Finder.TimeFreezeController.IsFrozen;

        private void Awake()
        {
            blockBoxCollider2D = transform.FindAnywhere(R.S.GameObject.Collider).GetComponent<BoxCollider2D>();
            playerSensor = GetComponentInChildren<Sensor>().For<Player>();
            sensor = GetComponentInChildren<Sensor>();
            isFalling = false;
        }

        private void Start()
        {
            //Anthony Bérubé
            playerSize = Finder.Player.Size;

            sensor.transform.localPosition = new Vector3(sensor.transform.localPosition.x,
                                                         -startingDistanceFromGround / 2, 0);
            sensorBoxCollider2D = transform.Find(R.S.GameObject.Sensor).GetComponent<BoxCollider2D>();
            sensorBoxCollider2D.size = new Vector2(blockBoxCollider2D.size.x
                                                 - blockBoxCollider2D.size.x * sensorXMargin,
                                                   startingDistanceFromGround);
            transform.position = new Vector3(transform.position.x, transform.position.y + startingDistanceFromGround,
                                             transform.position.z);
        }

        private void FixedUpdate()
        {
            //Anthony Bérubé
            if (isFalling && !IsFrozen)
                Fall();

            if (playerSensor.SensedObjects.Count > 0)
            {
                if (!IsFrozen)
                    isFalling = true;

                if (startingDistanceFromGround < playerSize)
                    playerSensor.SensedObjects[0].Die();
            }
        }

        private void Fall()
        {
            //Anthony Bérubé
            transform.position -= new Vector3(0, fallSpeed, 0);
            sensorBoxCollider2D.size = new Vector2(sensorBoxCollider2D.size.x, sensorBoxCollider2D.size.y - fallSpeed);
            sensor.transform.position = new Vector3(sensor.transform.position.x,
                                                    sensor.transform.position.y + fallSpeed / 2,
                                                    sensor.transform.position.z);

            if (Math.Abs(startingDistanceFromGround) > fallSpeed)
                startingDistanceFromGround -= fallSpeed;
            else
            {
                isFalling = false;
                enabled = false;
            }
        }
    }
}