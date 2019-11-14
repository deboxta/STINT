using System;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class FallingBlock : MonoBehaviour, IFreezable
    {
        //BC : Code mort.
        [SerializeField] private float fallSpeed = 5f;
        
        private new Rigidbody2D rigidbody2D;
        private DeadlyTrap deadlyTrap;
        private ISensor<Player> playerSensor;
        //BR: La fonctionalité de "TimeFreeze" est assez intrusive.
        //    Je suis d'avis que vos composants devrait supporter une méthode Freeze, ce que vous
        //    avez fait via "IFreezable".
        //
        //    Après cela, la question est "Comment avertir tous les objets "IFreezable" qu'ils devraient
        //    être "Freezed". J'ai deux solutions possibles :
        //        1. Tous les objets "IFreezable" on la responsabilité de s'ajouter à une liste de "Freezables"
        //           qui serait globale. Lorsque le temps "Freeze", cette liste serait parcourue pour "Freezer"
        //           tous les objets.
        //        2. Ajouter un composant avec tous les objets qui sont "Freezable" dont l'objetif est d'appeller
        //           "Freeze" et "Unfreeze" sur le "IFreezable" avec qui il partage le même GameObject. Vous utilisez
        //           le canal événemtiel pour savoir quand "Freeze" et "UnFreeze".
        //
        //     EDIT : OH, je viens de voir que "IFreezable" n'a pas de méthode "Freeze" ou "Unfreeze"...
        //            Faudrait en ajouter.
        private TimeFreezeEventChannel timeFreezeEventChannel;
        private Vector2 velocityBeforeFreeze;
        private bool wasKinematicBeforeFreeze;

        public bool Frozen => Finder.TimeFreezeController.IsFrozen;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            playerSensor = GetComponentInChildren<Sensor>().For<Player>();
            deadlyTrap = GetComponentInChildren<DeadlyTrap>();
            timeFreezeEventChannel = Finder.TimeFreezeEventChannel;
            velocityBeforeFreeze = Vector2.zero;
            wasKinematicBeforeFreeze = true;
        }

        private void OnEnable()
        {
            timeFreezeEventChannel.OnTimeFreezeStateChanged += OnTimeFreezeStateChanged;
            playerSensor.OnSensedObject += OnPlayerSensed;
        }

        private void OnDisable()
        {
            timeFreezeEventChannel.OnTimeFreezeStateChanged -= OnTimeFreezeStateChanged;
            playerSensor.OnSensedObject -= OnPlayerSensed;
        }

        private void OnTimeFreezeStateChanged()
        {
            if (Frozen)
            {
                //BC : Ces deux lignes devraient faire partie de "StopFalling".
                velocityBeforeFreeze = rigidbody2D.velocity;
                wasKinematicBeforeFreeze = rigidbody2D.isKinematic;
                StopFalling();
            }
            else
            {
                //BC : Il semble y avoir un problème de séparation en méthodes.
                //     Tu l'as fait pour "StopFalling", mais pas ici.
                rigidbody2D.velocity = velocityBeforeFreeze;
                rigidbody2D.isKinematic = wasKinematicBeforeFreeze;
                if (!rigidbody2D.isKinematic)
                    deadlyTrap.enabled = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var otherParentTransform = other.transform.parent;
            
            //BC : Pourquoi il colisionnerait avec lui même ?
            //     Pas logique.
            if (!IsSelf(otherParentTransform))
            {
                StopFalling();
            }
        }
        
        private bool IsSelf(Transform otherParentTransform)
        {
            return transform == otherParentTransform;
        }

        private void Fall()
        {
            //BC : Tu ne restore pas la vélocité ?
            rigidbody2D.isKinematic = false;
        }

        //BR : Voilà notre méthode "Freeze". Il manque une méthode "UnFreeze" qui s'assure de redonner
        //     la vélocité lorsque l'objet s'il était en train de descendre.
        private void StopFalling()
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.isKinematic = true;
            
            deadlyTrap.enabled = false;
        }
        
        private void OnPlayerSensed(Player player)
        {
            //BR : L'appel à "Fall" pourrait être "Idempotent".
            if (!Frozen)
                Fall();
        }
    }
}