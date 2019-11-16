using System;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Jeammy Côté
    public class SpringJump : MonoBehaviour
    {
        [Header("Configuration of spring jump")]
        [SerializeField] private float springForce = 10;
        [SerializeField] private float springDetectionDistance = 1f;
        
        private LayerMask layerToHit;
        private Transform raycastSensorLeft;
        private Transform raycastSensorRight;
        private RaycastHit2D hitLeft;
        private RaycastHit2D hitRight;

        private void Awake()
        {
            raycastSensorLeft = transform.Find("RaycastSensorLeft");
            raycastSensorRight = transform.Find("RaycastSensorRight");
        }

        private void Start()
        {
            layerToHit = 1 << LayerMask.NameToLayer(R.S.Layer.Player);
        }

        private void FixedUpdate()
        {
            hitLeft = Physics2D.Raycast(
                raycastSensorLeft.position, 
                transform.up, 
                springDetectionDistance, 
                layerToHit);
                        
            hitRight = Physics2D.Raycast(
                raycastSensorRight.position, 
                transform.up,
                springDetectionDistance,
                layerToHit);


            if (hitLeft && hitRight)
            {
                GameObject objectToSpringJump = hitLeft.transform.gameObject;
                if (objectToSpringJump)
                    SpringPlayer(objectToSpringJump);
            }
        }

        private void SpringPlayer(GameObject gameObject)
        {
            Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
            rigidbody.velocity = new Vector2(x: rigidbody.velocity.x, springForce);
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (raycastSensorLeft != null && raycastSensorRight != null)
            {
                Gizmos.DrawLine(raycastSensorLeft.position, raycastSensorLeft.position + springDetectionDistance * transform.localScale.y * transform.up);
                Gizmos.DrawLine(raycastSensorRight.position, raycastSensorRight.position + springDetectionDistance * transform.localScale.y * transform.up);
            }
        }
#endif
    }
}