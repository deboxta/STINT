using System;
using System.Collections;
using Boo.Lang;
using Harmony;
using UnityEngine;

namespace Game
{
    public class MovablePlatform : MonoBehaviour
    {
        [SerializeField] private float platformSpeed = 0.5f;
        [SerializeField] private Transform maxDistancePoint;
        [SerializeField] private Transform respawnPoint;
        [SerializeField] private float distanceToResetFromWall = 1f;
        [SerializeField] private float distanceToCatchPlatform = 1f;
        private LayerMask movablePlatformLayer;
        private RaycastHit2D hit;

        private void Awake()
        {
            maxDistancePoint = transform.Find("MaxDistancePoint");
            respawnPoint = transform.Find("RespawnPoint");
            movablePlatformLayer = LayerMask.GetMask(R.S.Layer.MovablePlatform);
        }

        private void FixedUpdate()
        {
            resetPlatformOnHit();
            
            foreach (var children in transform.Children())
            {
                if (LayerMask.LayerToName(children.layer) == R.S.Layer.MovablePlatform)
                {
                    children.GetComponent<Rigidbody2D>().velocity = new Vector2(platformSpeed,0);
                    //children.transform.position = new Vector2(children.transform.position.x + platformSpeed,children.transform.position.y);
                }
            }
        }

        private void resetPlatformOnHit()
        {
            hit = Physics2D.Raycast(maxDistancePoint.position,Vector2.left, distanceToResetFromWall, movablePlatformLayer);

            if (hit)
            {
                hit.transform.position = new Vector2(respawnPoint.position.x,respawnPoint.position.y);
            }
        }

#if UNITY_EDITOR
        //Author : Jeammy Côté
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(maxDistancePoint.position, new Vector2(maxDistancePoint.position.x + distanceToCatchPlatform, maxDistancePoint.position.y));
            Gizmos.DrawLine(respawnPoint.position, new Vector2(respawnPoint.position.x + distanceToResetFromWall, respawnPoint.position.y));
        }
#endif     
    }
}