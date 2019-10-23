using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class Box : MonoBehaviour
    {
        [SerializeField] private Timeline timeOfBox;
        [SerializeField] private Box boxFutureReference;
        [SerializeField] private int throwedForceUp = 40;
        [SerializeField] private int throwedForceX = 25;


        private Rigidbody2D rigidbody2D;
        private Collider2D boxCollider2D;
        private SpriteRenderer[] spriteRenderer;
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private Stimuli stimuli;
        private Vector3 positionPastBox;
        private TimelineController timelineController;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            rigidbody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
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

        private void DeActivateComponents()
        {
            rigidbody2D.simulated = false;
            boxCollider2D.enabled = false;
            spriteRenderer[0].enabled = false;
            stimuli.enabled = false;
        }

        private void ActivateComponents()
        {
            rigidbody2D.simulated = true;
            boxCollider2D.enabled = true;
            spriteRenderer[0].enabled = true;
            stimuli.enabled = true;
        }

        private void OnTimeLineChange()
        {
            if (timelineController.CurrentTimeline != timeOfBox)
            {
                DeActivateComponents();
            }
            else
            {
                ActivateComponents();
            }

            BoxParadoxRelationPosition();
        }

        private void BoxParadoxRelationPosition()
        {
            if (boxFutureReference != null)
            {
                if (Math.Abs(Position.x - positionPastBox.x) >= 0.1 ||
                    Math.Abs(Position.y - positionPastBox.y) >= 0.1)
                    boxFutureReference.Position = Position;

                positionPastBox = Position;
            }
        }

        public void Grabbed()
        {
            rigidbody2D.simulated = false;
            transform.localPosition = Vector3.zero;
        }

        public void Throwed(bool isLookingRight)
        {
            transform.parent = null;
            rigidbody2D.simulated = true;
            if (isLookingRight)
            {
                rigidbody2D.velocity = new Vector2(throwedForceX, throwedForceUp);
            }
            else
            {
                rigidbody2D.velocity = new Vector2(-throwedForceX, throwedForceUp);
            }
        }

        public void Dropped()
        {
            transform.parent = null;
            rigidbody2D.simulated = true;
        }
    }
}