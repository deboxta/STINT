using System;
using Harmony;
using UnityEngine;

namespace Game
{
    // Legacy
    // Author : Mathieu Boutet
    public class SegmentedLaser : Laser, IFreezable
    {
        [Range(0, 1000)] [SerializeField] private int nbMaxSegments = 100;
        [Range(0, 50)] [SerializeField] private float segmentSize = 1;
        [Range(0, 50)] [SerializeField] private float gapSize = 1;
        [Range(-50, 50)] [SerializeField] private float movementSpeed = 1;

        private LineRenderer[] laserBeamSegments;
        private float currentOffset;
        private int nbActiveSegments;

        public bool IsFrozen => Finder.TimeFreezeController.IsFrozen;

        private bool PlayerIsTouchingSegment
        {
            get
            {
                if (CastTouchesPlayer)
                {
                    var playerRaycastHit = RaycastHits[0];
                    for (int i = 0; i < nbActiveSegments; i++)
                    {
                        if (laserBeamSegments[i].GetPosition(1).IsPast(playerRaycastHit.point,
                                                                       transform.right, 0)
                         && (laserBeamSegments[i].GetPosition(0).IsBefore(playerRaycastHit.point,
                                                                          transform.right, 0)
                          || playerRaycastHit.collider.OverlapPoint(laserBeamSegments[i].GetPosition(0))))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        
        protected override void Awake()
        {
            base.Awake();

            laserBeamSegments = new LineRenderer[nbMaxSegments];
            laserBeamLineRenderer.gameObject.SetActive(false);
            for (int i = 0; i < nbMaxSegments; i++)
            {
                laserBeamSegments[i] = Instantiate(laserBeamLineRenderer, transform);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            int nbSegmentsNeededLastUpdate = nbActiveSegments;
            nbActiveSegments =
                Math.Max(0, Math.Min(nbMaxSegments,
                                     (int) Math.Ceiling(transform.position.DistanceTo(LaserBeamEndPosition)
                                                      / (segmentSize + gapSize) + 2)));

            for (int i = nbActiveSegments; i < nbSegmentsNeededLastUpdate; i++)
            {
                laserBeamSegments[i].gameObject.SetActive(false);
            }

            float initialOffset = -(segmentSize + gapSize);
            Vector2 segmentStartPosition;
            Vector2 segmentEndPosition;
            for (int i = 0; i < nbActiveSegments; i++)
            {
                laserBeamSegments[i].gameObject.SetActive(true);
                segmentStartPosition = transform.position + transform.right
                                     * (i * (segmentSize + gapSize) + currentOffset);

                segmentEndPosition = segmentStartPosition + (Vector2) transform.right * segmentSize;
                if (segmentStartPosition.IsBefore(transform.position, transform.right, 0))
                {
                    segmentStartPosition = transform.position;
                    if (segmentEndPosition.IsBefore(transform.position, transform.right, 0))
                        segmentEndPosition = transform.position;
                }

                if (segmentEndPosition.IsPast(LaserBeamEndPosition, transform.right, 0))
                {
                    segmentEndPosition = LaserBeamEndPosition;
                    if (segmentStartPosition.IsPast(LaserBeamEndPosition, transform.right, 0))
                        segmentStartPosition = LaserBeamEndPosition;
                }
                
                laserBeamSegments[i].SetPosition(0, segmentStartPosition);
                laserBeamSegments[i].SetPosition(1, segmentEndPosition);
            }

            if (PlayerIsTouchingSegment)
                Finder.Player.Die();

            if (segmentSize + gapSize != 0 && !IsFrozen)
                currentOffset = (currentOffset + (movementSpeed * Time.fixedDeltaTime)) % (segmentSize + gapSize)
                              + initialOffset;
        }
    }
}