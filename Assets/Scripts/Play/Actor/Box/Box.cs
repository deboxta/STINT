using System;
using Harmony;
using UnityEngine;

namespace Game
{
     public class Box : MonoBehaviour
    {
        public enum TimeOfBox
        {
            past,
            future
        }

        public TimeOfBox timeOfBox;

        public Timeline boxTimeline;
        
        [SerializeField] private Box boxFutureReference;
        [SerializeField] private int throwedForceUp = 40;
        [SerializeField] private int throwedForceX = 25;
        
        
        private Rigidbody2D rigidbody2D;
        private Collider2D boxCollider2D;
        private SpriteRenderer spriteRenderer;
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private Stimuli stimuli;
        private Vector2 positionFutureBox;
        public Vector3 Position => transform.position;
        

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            rigidbody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            stimuli = GetComponentInChildren<Stimuli>();
            if (boxTimeline == Timeline.Secondary)
            {
               DeActivateComponents();
            }
        }

        private void OnEnable()
        {
            timelineChangedEventChannel.OnTimelineChanged += OnTimeLineChange;
        }

        private void OnDisable()
        {
            timelineChangedEventChannel.OnTimelineChanged -= OnTimeLineChange;
        }
        
        protected void DeActivateComponents()
        {
            rigidbody2D.simulated = false;
            boxCollider2D.enabled = false;
            spriteRenderer.enabled = false;
            stimuli.enabled = false;
        }

        protected void ActivateComponents()
        {
            rigidbody2D.simulated = true;
            boxCollider2D.enabled = true;
            spriteRenderer.enabled = true;
            stimuli.enabled = true;
        }

        virtual protected void OnTimeLineChange()
        {
            switch (Finder.TimelineController.CurrentTimeline)
            {
                case Timeline.Primary:
                    TimePrimary();
                    break;
                case Timeline.Secondary:
                    TimeSecondary();
                    break;
            }
        }

        private void TimePrimary()
        {
            if (boxTimeline == Timeline.Secondary)
            {
                DeActivateComponents();
            }
            else
            {
                ActivateComponents();
            }
            if (timeOfBox == TimeOfBox.future)
            BoxParadoxRelationPosition();
        }
        
        private void TimeSecondary()
        {
            if (boxTimeline == Timeline.Primary)
            {
                DeActivateComponents();
            }
            else
            {
                ActivateComponents();
            }
        }
        
        private void BoxParadoxRelationPosition()
        {
            /*if (Math.Abs(Position.x - positionFutureBox.x) >= 0.5f || 
                Math.Abs(Position.y - positionFutureBox.y) >= 0.5f)
            {
                boxFutureReference.transform.position = Position;
                boxFutureReference.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }      */  }

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
    }
}