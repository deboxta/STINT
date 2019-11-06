using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Game
{
    public class TransitionZone : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] private CompositeCollider2D roomOne;
        [SerializeField] private CompositeCollider2D roomTwo;

        private ISensor<Player> playerSensor;
        private BoxCollider2D transitionBox;

        private void Awake()
        {
            playerSensor = GetComponent<Sensor>().For<Player>();
        }

        private IEnumerator Start()
        {
            yield return null;

            transitionBox = GetComponent<BoxCollider2D>();
        }

        private void OnEnable()
        {
            playerSensor.OnSensedObject += OnPlayerSensed;
            playerSensor.OnSensedObject += OnPlayerUnSensed;
        }

        private void OnDisable()
        {
            playerSensor.OnSensedObject -= OnPlayerSensed;
            playerSensor.OnSensedObject -= OnPlayerUnSensed;
        }

        private void OnPlayerSensed(Player sensedPlayer)
        {
            player.GetComponent<Rigidbody2D>().isKinematic = true;

            var cinemachineConfiner = cinemachineVirtualCamera.GetComponent<CinemachineConfiner>();
            cinemachineConfiner.enabled = false;
        }

        private void OnPlayerUnSensed(Player sensedPlayer)
        {
            player.GetComponent<Rigidbody2D>().isKinematic = false;

            var cinemachineConfiner = cinemachineVirtualCamera.GetComponent<CinemachineConfiner>();
            cinemachineConfiner.m_BoundingShape2D =
                (cinemachineConfiner.m_BoundingShape2D == roomOne) ? roomTwo : roomOne;

            cinemachineConfiner.enabled = true;
        }

        /*private IEnumerator MovePlayer()
        {
            
        }*/

    }
}