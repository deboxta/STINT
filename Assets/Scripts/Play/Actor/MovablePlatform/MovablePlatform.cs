using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Jeammy Côté
    public class MovablePlatform : MonoBehaviour
    {
        [SerializeField] private float platformSpeed = 0.5f;
        [SerializeField] private Transform maxDistancePoint;
        [SerializeField] private Transform respawnPoint;
        [SerializeField] private float distanceToResetFromWall = 1f;
        [SerializeField] private float distanceToCatchPlatform = 1f;
        
        private LayerMask movablePlatformLayer;
        private RaycastHit2D hit;
        private static readonly string MAX_DISTANCE_GAMEOBJECT_NAME = "MaxDistancePoint";
        private static readonly string RESPAWN_POINT_GAMEOBJECT_NAME = "RespawnPoint";

        private void Awake()
        {
            maxDistancePoint = transform.Find(MAX_DISTANCE_GAMEOBJECT_NAME);
            respawnPoint = transform.Find(RESPAWN_POINT_GAMEOBJECT_NAME);
            movablePlatformLayer = LayerMask.GetMask(R.S.Layer.MovablePlatform);
        }

        private void FixedUpdate()
        {
            resetPlatformOnHit();
            
            foreach (var children in transform.Children())
                if (LayerMask.LayerToName(children.layer) == R.S.Layer.MovablePlatform)
                    children.GetComponent<Rigidbody2D>().velocity = new Vector2(platformSpeed,0);
        }

        private void resetPlatformOnHit()
        {
            hit = Physics2D.Raycast(maxDistancePoint.position,Vector2.left, distanceToResetFromWall, movablePlatformLayer);

            if (hit)
                hit.transform.position = new Vector2(respawnPoint.position.x,respawnPoint.position.y);
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