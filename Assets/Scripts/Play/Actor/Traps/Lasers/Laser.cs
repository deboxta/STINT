﻿using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public abstract class Laser : MonoBehaviour
    {
        protected const int LASER_BEAM_DEFAULT_LENGTH = 500;
        protected const int RAYCAST_HITS_BUFFER_SIZE = 50;
        protected const float RAYCAST_ORIGIN_DEAD_ZONE = 0.05f;

        protected LineRenderer laserBeam;
        protected bool CastTouchesPlayer { get; private set; }
        protected Vector3 LaserBeamStartPosition { get; private set; }
        protected Vector3 LaserBeamEndPosition { get; private set; }
        protected RaycastHit2D[] RaycastHits { get; private set; }
        protected int NbRaycastHits { get; private set; }
        protected int LayersToHit { get; private set; }

        protected virtual void Awake()
        {
            RaycastHits = new RaycastHit2D[RAYCAST_HITS_BUFFER_SIZE];
            laserBeam = this.GetRequiredComponentInChildren<LineRenderer>(true);
            laserBeam.useWorldSpace = true;
            CastTouchesPlayer = false;
            
            //https://answers.unity.com/questions/416919/making-raycast-ignore-multiple-layers.html
            //To add a layer to hit do : LayersToHit = |= (1 << LayerMask.NameToLayer(LayerName));
            //Author : Sébastien Arsenault
            LayersToHit = (1 << LayerMask.NameToLayer(R.S.Layer.Floor));
            LayersToHit |= (1 << LayerMask.NameToLayer(R.S.Layer.Player));
        }

        protected virtual void FixedUpdate()
        {
            NbRaycastHits = Physics2D.RaycastNonAlloc(transform.position 
                                                    + transform.right * RAYCAST_ORIGIN_DEAD_ZONE,
                                                      transform.right,
                                                      RaycastHits, LASER_BEAM_DEFAULT_LENGTH, LayersToHit);
            
            CastTouchesPlayer = false;
            int blockingObjectIndex = -1;
            if (NbRaycastHits > 0)
            {
                if (RaycastHits[0].transform.root.CompareTag(R.S.Tag.Player))
                {
                    CastTouchesPlayer = true;
                    for (int i = 1; i < NbRaycastHits; i++)
                    {
                        if (!RaycastHits[i].transform.root.CompareTag(R.S.Tag.Player))
                        {
                            blockingObjectIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    blockingObjectIndex = 0;
                }
            }

            LaserBeamStartPosition = transform.position;

            if (blockingObjectIndex >= 0)
                LaserBeamEndPosition = RaycastHits[blockingObjectIndex].point;
            else
                LaserBeamEndPosition = transform.position + transform.right * LASER_BEAM_DEFAULT_LENGTH;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color darkRed = Color.Lerp(Color.red, Color.black, 0.5f);
            
            // Raycast hit points
            Gizmos.color = Color.red;
            if (RaycastHits != null)
            {
                for (int i = 0; i < NbRaycastHits; i++)
                {
                    Gizmos.DrawSphere(RaycastHits[i].point, 0.1f);
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