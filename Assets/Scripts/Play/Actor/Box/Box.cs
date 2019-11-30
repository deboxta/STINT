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


        private new Rigidbody2D rigidbody2D;
        private Collider2D boxCollider2D;
        private SpriteRenderer spriteRenderer;
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private Stimuli stimuli;
        private Vector3 positionPastBox;
        private TimelineController timelineController;
        private Transform originalParent;

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
            originalParent = transform.parent;
        }

        private void OnEnable()
        {
            timelineChangedEventChannel.OnTimelineChanged += OnTimelineChange;
        }

        private void OnDisable()
        {
            timelineChangedEventChannel.OnTimelineChanged -= OnTimelineChange;
        }

        private void Hide()
        {
            rigidbody2D.simulated = false;
            boxCollider2D.enabled = false;
            spriteRenderer.enabled = false;
            stimuli.enabled = false;
        }

        private void Show()
        {
            rigidbody2D.simulated = true;
            boxCollider2D.enabled = true;
            spriteRenderer.enabled = true;
            stimuli.enabled = true;
        }

        private void OnTimelineChange()
        {
            if (timelineController.CurrentTimeline != timeOfBox)
                Hide();
            else
                Show();

            BoxParadoxRelationPosition();
        }

        private void BoxParadoxRelationPosition()
        {
            if (boxFutureReference != null)
            {
                if (PastBoxMoved())
                    boxFutureReference.Position = Position;

                positionPastBox = Position;
            }
        }

        private bool PastBoxMoved()
        {
            return Math.Abs(Position.x - positionPastBox.x) >= 0.2 ||
                   Math.Abs(Position.y - positionPastBox.y) >= 0.2;
        }

        public void Grab()
        {
            rigidbody2D.simulated = false;
            transform.localPosition = Vector3.zero;
        }

        public void Throw(bool isLookingRight)
        {
            transform.parent = originalParent;
            rigidbody2D.simulated = true;
            if (isLookingRight)
                rigidbody2D.velocity = new Vector2(throwedForceX, throwedForceUp);
            else
                rigidbody2D.velocity = new Vector2(-throwedForceX, throwedForceUp);
        }

        public void Drop()
        {
            transform.parent = originalParent;
            rigidbody2D.simulated = true;
        }
    }
}