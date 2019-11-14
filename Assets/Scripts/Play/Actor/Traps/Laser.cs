using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public abstract class Laser : MonoBehaviour
    {
        //BC : Devrait être un "SerializedField", car on veut pouvoir modfier cela dans l'éditeur.
        protected const int LASER_BEAM_DEFAULT_LENGTH = 500;
        //BC : Devrait être un "SerializedField", car on veut pouvoir modfier cela dans l'éditeur.
        protected const int RAYCAST_HITS_BUFFER_SIZE = 50;
        //BC : Devrait être un "SerializedField", car on veut pouvoir modfier cela dans l'éditeur.
        protected const float RAYCAST_ORIGIN_DEAD_ZONE = 0.05f;

        //BC : Nommage. C'est un "Renderer". Cela devrait paraitre dans le nom.
        protected LineRenderer laserBeam;
        //BR : Ça, c'est ce que j'appelle un "Bad Smell". Juste à voir cette propriété, je sais qu'il y a une erreur
        //     de design à quelque part. Un événement qui est pas envoyé au bon momment ou quelque chose du genre.
        //     EDIT : Et j'avais raison!
        protected bool CastTouchesPlayer { get; private set; }
        //BR : Un autre bad smell.
        protected Vector3 LaserBeamStartPosition { get; private set; }
        protected Vector3 LaserBeamEndPosition { get; private set; }
        protected RaycastHit2D[] RaycastHits { get; private set; }
        protected int NbRaycastHits { get; private set; }

        protected virtual void Awake()
        {
            //BR : Pourrait être un attribut. Par contre, si c'est un attribut, devrait être private.
            //     De toute façon, avec tout le refactoring à faire ici, c'est probablement ce qui va arriver.
            RaycastHits = new RaycastHit2D[RAYCAST_HITS_BUFFER_SIZE];
            laserBeam = GetComponentInChildren<LineRenderer>();
            laserBeam.useWorldSpace = true;
            CastTouchesPlayer = false;
        }

        protected virtual void FixedUpdate()
        {
            //BR : Bonne idée. Plus performant en effet, surtout lorsque c'est fait régulièrement.
            NbRaycastHits = Physics2D.RaycastNonAlloc(transform.position 
                                                    + transform.right * RAYCAST_ORIGIN_DEAD_ZONE,
                                                      transform.right,
                                                      RaycastHits);
            
            CastTouchesPlayer = false;
            int blockingObjectIndex = -1;
            if (NbRaycastHits > 0)
            {
                if (RaycastHits[0].transform.root.CompareTag(R.S.Tag.Player))
                {
                    //BC : En lien avec un autre commentaire, le joueur devrait être tué ici, et non
                    //     pas dans la classe enfant "ConstantLaser".
                    CastTouchesPlayer = true;
                    //BC : Logique applicative fragile. Pourquoi est-ce que vous avez décidé que le laser passe au travers
                    //     du joueur ?
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

            //BC : Au lieu de stocker la position de départ du laser ici pour l'assigner dans le "LineRenderer"
            //     plus loin, tu devrais assigner la valeur au line renderer directement ici. Logique applicative fragile.
            LaserBeamStartPosition = transform.position;

            if (blockingObjectIndex >= 0)
                LaserBeamEndPosition = RaycastHits[blockingObjectIndex].point;
            else
                LaserBeamEndPosition = transform.position + transform.right * LASER_BEAM_DEFAULT_LENGTH;
        }

        //BR : Je ne corrige pas ce bout là. Code d'éditeur. Ignoré
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