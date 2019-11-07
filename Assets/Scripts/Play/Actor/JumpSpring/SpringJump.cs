using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class SpringJump : MonoBehaviour
    {
        //Boolean to see if the player is in jump spring zone.
        [SerializeField] private bool hitLeft;
        [SerializeField] private bool hitRight;
        
        [SerializeField] private float springForce = 10;
        [SerializeField] private float springDetectionDistance = 1f;
        [SerializeField] private LayerMask playerLayer;
        
        private Transform raycastSensorLeft;
        private Transform raycastSensorRight;
        
        private void Awake()
        {
            raycastSensorLeft = transform.Find("RaycastSensorLeft");
            raycastSensorRight = transform.Find("RaycastSensorRight");
        }

        private void Update()
        {
            hitLeft = Physics2D.Raycast(
                raycastSensorLeft.position, 
                transform.up, 
                springDetectionDistance, 
                playerLayer);
                        
            hitRight = Physics2D.Raycast(
                raycastSensorRight.position, 
                transform.up,
                springDetectionDistance,
                playerLayer);
            
            if (hitLeft && hitRight)
                OnPlayerSensed();
        }

        private void OnPlayerSensed()
        {
            Finder.Player.SpringJump(springForce);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (raycastSensorLeft != null && raycastSensorRight != null)
            {
                Gizmos.DrawLine(raycastSensorLeft.position, raycastSensorLeft.position + springDetectionDistance * transform.localScale.y * transform.up);
                Gizmos.DrawLine(raycastSensorRight.position, raycastSensorRight.position + springDetectionDistance * transform.localScale.y * transform.up);
            }
        }
    }
}