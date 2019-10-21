using System;
using Boo.Lang;
using UnityEngine;

namespace Game
{
    public class Laser : MonoBehaviour
    {
        private const int BEAM_MAX_LENGTH = 999;
        
        private LineRenderer laserBeam;
        private RaycastHit2D[] raycastHits;

        private void Awake()
        {
            raycastHits = new RaycastHit2D[0];
            laserBeam = GetComponent<LineRenderer>();
            laserBeam.useWorldSpace = true;
        }

        private void Start()
        {
//            raycastHits = Physics2D.RaycastAll(transform.position, transform.right);
        }

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            raycastHits = Physics2D.RaycastAll(transform.position, transform.right);
            // Visual has to update after the raycast or the beam's end
            // point stays at the last position when moving it fast
            laserBeam.SetPosition(0, transform.position);
            if (raycastHits != null && raycastHits.Length != 0)
            {
                laserBeam.SetPosition(1, raycastHits[0].point);
            }
            else
            {
                laserBeam.SetPosition(1, transform.position + transform.right * BEAM_MAX_LENGTH);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Beam trajectory
            Gizmos.color = Color.Lerp(Color.red, Color.black, 0.5f);
            Gizmos.DrawLine(transform.position, transform.position + transform.right * BEAM_MAX_LENGTH);
            // Raycast hit points
            if (raycastHits != null)
            {
                foreach (var raycastHit in raycastHits)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(raycastHit.point, 0.1f);
                }
            }

            // Reset color
            Gizmos.color = Color.white;
        }
#endif
    }
}