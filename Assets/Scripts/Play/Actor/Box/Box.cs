using System;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Anthony Bérubé
    
    public class Box : MonoBehaviour
    {
        [SerializeField] private Timeline timeOfBox;
        [SerializeField] private Box boxFutureReference;
        [SerializeField] private int throwedForceUp = 40;
        [SerializeField] private int throwedForceX = 25;


        private Rigidbody2D rigidbody2D;
        private Collider2D boxCollider2D;
        private SpriteRenderer spriteRenderer;
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private Stimuli stimuli;
        private Vector3 positionPastBox;
        private TimelineController timelineController;

        //BR : Excellent.
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            rigidbody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = transform.Find(R.S.GameObject.VisualMain).GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            stimuli = GetComponentInChildren<Stimuli>();
            timelineController = Finder.TimelineController;
        }

        private void OnEnable()
        {
            timelineChangedEventChannel.OnTimelineChanged += OnTimeLineChange;
        }

        private void OnDisable()
        {
            timelineChangedEventChannel.OnTimelineChanged -= OnTimeLineChange;
        }

        //BR : Renommer en "DesactivateComponents".
        //     En fait, j'irais même plus loin en renommant cela en "Hide".
        private void DeActivateComponents()
        {
            rigidbody2D.simulated = false;
            boxCollider2D.enabled = false;
            spriteRenderer.enabled = false;
            stimuli.enabled = false;
        }

        //BR : Placer cette méthode "avant" "DeActivateComponents".
        //BR : Renommer pour "Show".
        private void ActivateComponents()
        {
            rigidbody2D.simulated = true;
            boxCollider2D.enabled = true;
            spriteRenderer.enabled = true;
            stimuli.enabled = true;
        }

        private void OnTimeLineChange()
        {
            if (timelineController.CurrentTimeline != timeOfBox)
                DeActivateComponents();
            else
                ActivateComponents();

            BoxParadoxRelationPosition();
        }

        private void BoxParadoxRelationPosition()
        {
            if (boxFutureReference != null)
            {
                //BR : Utiliser une méthode d'extension pour savoir si deux position
                //     sont à peu près les mêmes.
                if (Math.Abs(Position.x - positionPastBox.x) >= 0.1 ||
                    Math.Abs(Position.y - positionPastBox.y) >= 0.1)
                    boxFutureReference.Position = Position;

                //BC : Es-tu certain que tu ne devrais pas assigner cette valeur une première fois au "Awake" aussi ?
                positionPastBox = Position;
            }
        }

        //BR : Nommage. Grab.
        public void Grabbed()
        {
            rigidbody2D.simulated = false;
            transform.localPosition = Vector3.zero;
        }

        //BC : Devrait recevoir en paramètre une direction sous la forme d'une Vector2 (ou un Vector3)
        //     ainsi qu'une force. Actuellement, la boite sait la force avec laquelle elle est lancée.
        //BR : Nommage. Throw.
        public void Throwed(bool isLookingRight)
        {
            transform.parent = null;
            rigidbody2D.simulated = true;
            if (isLookingRight)
                rigidbody2D.velocity = new Vector2(throwedForceX, throwedForceUp);
            else
                rigidbody2D.velocity = new Vector2(-throwedForceX, throwedForceUp);
        }

        //BR : Nommage. Drop.
        public void Dropped()
        {
            transform.parent = null;
            rigidbody2D.simulated = true;
        }
    }
}