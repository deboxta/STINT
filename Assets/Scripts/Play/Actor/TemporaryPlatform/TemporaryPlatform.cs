using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    //Author : Sébastien Arsenault
    public class TemporaryPlatform : MonoBehaviour
    {
        [SerializeField] [Range(1, 20)] private float duration = 5;
        
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private TilemapRenderer tileMapRenderer;
        private Rigidbody2D rigidBody2D;
        private CompositeCollider2D compositeCollider2D;
        private TilemapCollider2D tileMapCollider2D;

        private bool ignoreFirstTimelineChanged;

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            tileMapRenderer = GetComponent<TilemapRenderer>();
            rigidBody2D = GetComponent<Rigidbody2D>();
            compositeCollider2D = GetComponent<CompositeCollider2D>();
            tileMapCollider2D = GetComponent<TilemapCollider2D>();
            
            Deactivate();

            ignoreFirstTimelineChanged = true;
        }

        private void OnEnable()
        {
            timelineChangedEventChannel.OnTimelineChanged += OnTimelineChangedEventChannel;
        }
        
        private void OnDisable()
        {
            timelineChangedEventChannel.OnTimelineChanged -= OnTimelineChangedEventChannel;
        }

        private void Deactivate()
        {
            tileMapRenderer.enabled = false;
            rigidBody2D.simulated = false;
            compositeCollider2D.isTrigger = true;
            tileMapCollider2D.isTrigger = true;
        }
        
        private void Activate()
        {
            tileMapRenderer.enabled = true;
            rigidBody2D.simulated = true;
            compositeCollider2D.isTrigger = false;
            tileMapCollider2D.isTrigger = false;
        }

        private IEnumerator Appear()
        {
            Activate();
            yield  return new WaitForSeconds(duration);
            Deactivate();
        }

        private void OnTimelineChangedEventChannel()
        {
            if (ignoreFirstTimelineChanged)
            {
                ignoreFirstTimelineChanged = false;
                return;
            }
            StartCoroutine(Appear());
        }
    }
}