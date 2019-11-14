using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Anthony Bérubé
    
    [Findable(R.S.Tag.Player)]
    [RequireComponent(typeof(PlayerMover), typeof(PlayerInput))]
    public class Player : MonoBehaviour , IPowerUpCollector
    {
        private PlayerDeathEventChannel playerDeathEventChannel;
        private Sensor sensor;
        private ISensor<Box> boxSensor;
        private Hands hands;
        private bool isLookingRight;
        private Vitals vitals;
        private bool isCrouched;
        private PlayerMover playerMover;

        public Hands Hands => hands;
        public Vitals Vitals => vitals;
        public bool IsDead { get; set; }

        //BR : Ça ne me plait vraiment pas ça....
        public bool IsLookingRight 
        { 
            set => isLookingRight = value;
        }

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;

            hands = GetComponentInChildren<Hands>();
            sensor = GetComponentInChildren<Sensor>();
            vitals = GetComponentInChildren<Vitals>();
            playerMover = GetComponent<PlayerMover>();
            
            isLookingRight = true;
            IsDead = false;
            isCrouched = false;
            
            boxSensor = sensor.For<Box>();
        }

        //Author : Jeammy Côté
        //Change player direction
        public void FlipPlayer()
        {
            //BC : Il y a une variable "isLookingRight". C'est de l'information dupliquée.
            //BC : Constantes manquantes (pour new Vector2(-1,1)). Ici, tu devras faire un "static readonly".
            transform.localScale = transform.localScale.x == 1 ? new Vector2(-1, 1) : Vector2.one;
        }
        
        //Author : Sébastien Arsenault
        [ContextMenu("Die")]
        public void Die()
        {
            if (!IsDead)
            {
                IsDead = true;
                playerDeathEventChannel.NotifyPlayerDeath();
            }
        }

        //TODO : LOOK FOR THE NEAREST BOX IN CASE THERE'S TWO AND THE DIRECTION
        //Grabs the box
        public void GrabBox()
        {
            //BR : Ce commentaire peut être remplacé par du code en utilisant des proptiétésé
            //     Par exemple :
            //         if (!IsHoldingBox && IsSensingBox)
            //     Ou   
            //         if (!hands.IsHoldingBox && IsSensingBox)
            //
            //     Je préfère la première option.

            //If the player isn't holding a box and if there is a box in his sensor
            if (!hands.IsHoldingBox && boxSensor.SensedObjects.Count > 0)
            {
                //Grabs the box
                //BR : Toujours en utilisant des propriétés
                //     hands.Grab(SensedBox);
                hands.Grab(boxSensor.SensedObjects[0]);
                playerMover.Slowed();
            }
        }
        
        //BC : Nommage mensonger. Cela ne lance pas toujours la boite.
        public void ThrowBox(bool crouching)
        {
            //BC : Logique d'"Input" à la mauvaise place. C'est pas à "Player" de faire cela.
            //     Tu devrais créer une méthode "DropBox".
            if (crouching)
                hands.Drop();
            else
                hands.Throw(isLookingRight);
            
            playerMover.ResetSpeed();
        }
        
        //Author : Jeammy Côté
        public void CollectPowerUp()
        {
            playerMover.ResetNumberOfJumpsLeft();
        }
        
        //Author : Jeammy Côté
        public void CollectBoots()
        {
            //BR : Avoir un "PlayerInventory" aiderait à gérer cela de manière centralisée.
            //     En plus, quand viendrait le temps d'effectuer les sauvegardes, il est plus facile de savoir
            //     où les données à sauvegarder sont stockées (et aussi où elles doivent être restorées).
            playerMover.HasBoots = true;
        }
    }
}