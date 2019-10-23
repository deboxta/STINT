using System;
using Boo.Lang;
using Harmony;
using UnityEngine;

namespace Game
{
    public class ConstantLaser : MonoBehaviour
    {
        private const int LASER_BEAM_DEFAULT_LENGTH = 999;
        private const int RAYCAST_HITS_BUFFER_SIZE = 50;
        private const float RAYCAST_ORIGIN_DEAD_ZONE = 0.05f;

        protected LineRenderer laserBeam;
        private RaycastHit2D[] raycastHits;
        private int nbRaycastHits;
        private bool firing;

        protected bool Firing
        {
            get => firing;
            set
            {
                firing = value;
                if (!value)
                    laserBeam.SetPosition(1, transform.position);
                laserBeam.gameObject.SetActive(value);
            }
        }

        private void Awake()
        {
            raycastHits = new RaycastHit2D[RAYCAST_HITS_BUFFER_SIZE];
            laserBeam = GetComponentInChildren<LineRenderer>();
            laserBeam.useWorldSpace = true;
            Firing = true;
        }

        protected void FixedUpdate()
        {
            if (!Firing) return;
            
            Vector3 laserBeamEndPosition;
            nbRaycastHits = Physics2D.RaycastNonAlloc(transform.position 
                                                    + transform.right * RAYCAST_ORIGIN_DEAD_ZONE,
                                                      transform.right,
                                                      raycastHits);

            int blockingObjectIndex = -1;
            if (nbRaycastHits > 0)
            {
                if (raycastHits[0].transform.root.CompareTag(R.S.Tag.Player))
                {
                    Finder.Player.Die();
                    for (int i = 1; i < nbRaycastHits; i++)
                    {
                        if (!raycastHits[i].transform.root.CompareTag(R.S.Tag.Player))
                        {
                            blockingObjectIndex = i;
                        }
                    }
                }
                else
                {
                    blockingObjectIndex = 0;
                }
            }

            if (blockingObjectIndex >= 0)
                laserBeamEndPosition = raycastHits[blockingObjectIndex].point;
            else
                laserBeamEndPosition = transform.position + transform.right * LASER_BEAM_DEFAULT_LENGTH;

            // Visual has to update after the raycast or the beam's end
            // point will stay at the last position when moving it fast
            laserBeam.SetPosition(0, transform.position);
            laserBeam.SetPosition(1, laserBeamEndPosition);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color darkRed = Color.Lerp(Color.red, Color.black, 0.5f);
            
            // Raycast hit points
            Gizmos.color = Color.red;
            if (raycastHits != null)
            {
                for (int i = 0; i < nbRaycastHits; i++)
                {
                    Gizmos.DrawSphere(raycastHits[i].point, 0.1f);
                }
            }
            
            // Beam trajectory
            Gizmos.color = darkRed;
            Gizmos.DrawRay(transform.position, transform.right * LASER_BEAM_DEFAULT_LENGTH);

            // Reset color
            Gizmos.color = Color.white;
        }
#endif
    }
}