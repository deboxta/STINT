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

        private LineRenderer laserBeam;
        private Vector3 laserBeamEndPosition;
        private RaycastHit2D[] raycastHits;
        private int nbRaycastHits;

        private void Awake()
        {
            raycastHits = new RaycastHit2D[RAYCAST_HITS_BUFFER_SIZE];
            laserBeam = GetComponent<LineRenderer>();
            laserBeam.useWorldSpace = true;
        }

        private void FixedUpdate()
        {
            nbRaycastHits = Physics2D.RaycastNonAlloc(transform.position, transform.right, raycastHits);

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
            // Beam trajectory
            Gizmos.color = Color.Lerp(Color.red, Color.black, 0.5f);
            Gizmos.DrawLine(transform.position, transform.position + transform.right * LASER_BEAM_DEFAULT_LENGTH);
            // Raycast hit points
            if (raycastHits != null)
            {
                for (int i = 0; i < nbRaycastHits; i++)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(raycastHits[i].point, 0.1f);
                }
            }

            // Reset color
            Gizmos.color = Color.white;
        }
#endif
    }
}