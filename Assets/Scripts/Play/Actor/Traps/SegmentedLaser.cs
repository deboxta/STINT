using System;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    //BC : Je crois que tu as essayé de trop réutiliser le code de la classe Laser, ce qui cause tout ces défauts
    //     de structure.
    //
    //     Tu peux te faire une classe de base pour les lasers, mais je crois qu'elle va contenir plus de méthodes
    //     abstraites que autre chose. Il y aura probablement pas grand chose de réutilisable.
    public class SegmentedLaser : Laser, IFreezable
    {
        [SerializeField] [Range(0, 1000)] private int nbMaxSegments = 100;
        [SerializeField] [Range(0, 50)] private float segmentSize = 1;
        [SerializeField] [Range(0, 50)] private float gapSize = 1;
        [SerializeField] [Range(-50, 50)] private float movementSpeed = 1;

        //BR : Il y a des constantes autogénérées justement pour ça;
        //     Voir R.S.GameObject.LaserBeamSegment;
        private const string SEGMENT_OBJECT_NAME = "LaserBeamSegment";

        //BC : Défaut dans l'héritage. Certaines parties de la classe "Laser" ne sont pas utilisées (voir laserBeam).
        //     Lorsque l'on fait de l'héritage, c'est pour ajouter des fonctionalités et non pas en remplacer.
        private LineRenderer[] laserBeamSegments;
        private float currentOffset;
        private int nbActiveSegments;

        private bool PlayerIsTouchingSegment
        {
            get
            {
                if (CastTouchesPlayer)
                {
                    var playerRaycastHit = RaycastHits[0];
                    for (int i = 0; i < nbActiveSegments; i++)
                    {
                        //BR : Bon usage de méthodes d'extension.
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

        public bool Frozen => Finder.TimeFreezeController.IsFrozen;

        protected override void Awake()
        {
            base.Awake();

            laserBeamSegments = new LineRenderer[nbMaxSegments];
        }

        private void Start()
        {
            for (int i = 0; i < nbMaxSegments; i++)
            {
                laserBeamSegments[i] = Instantiate(laserBeam, transform);
                laserBeamSegments[i].name = SEGMENT_OBJECT_NAME;
                laserBeamSegments[i].gameObject.SetActive(false);
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
            Vector3 segmentStartPosition;
            Vector3 segmentEndPosition;
            for (int i = 0; i < nbActiveSegments; i++)
            {
                laserBeamSegments[i].gameObject.SetActive(true);
                segmentStartPosition = transform.position + transform.right
                                     * (i * (segmentSize + gapSize) + currentOffset);

                segmentEndPosition = segmentStartPosition + transform.right * segmentSize;
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

            if (segmentSize + gapSize != 0 && !Frozen)
                currentOffset = (currentOffset + (movementSpeed * Time.fixedDeltaTime)) % (segmentSize + gapSize)
                              + initialOffset;
        }
    }
}