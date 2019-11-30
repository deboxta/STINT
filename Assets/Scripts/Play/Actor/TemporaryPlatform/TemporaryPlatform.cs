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
        private TilemapCollider2D tileMapCollider2D;

        private bool ignoreFirstTimelineChanged;

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            tileMapRenderer = GetComponent<TilemapRenderer>();
            rigidBody2D = GetComponent<Rigidbody2D>();
            tileMapCollider2D = GetComponent<TilemapCollider2D>();
            
            Deactivate();

            ignoreFirstTimelineChanged = true;
        }

        private void OnEnable()
        {
            Deactivate();
            timelineChangedEventChannel.OnTimelineChanged += OnTimelineChangedEventChannel;
        }
        
        private void OnDisable()
        {
            timelineChangedEventChannel.OnTimelineChanged -= OnTimelineChangedEventChannel;
            Deactivate();
        }

        private void Deactivate()
        {
            tileMapRenderer.enabled = false;
            rigidBody2D.simulated = false;
            tileMapCollider2D.enabled = false;
        }
        
        private void Activate()
        {
            tileMapRenderer.enabled = true;
            rigidBody2D.simulated = true;
            tileMapCollider2D.enabled = true;
        }

        private IEnumerator Appear()
        {
            Activate();
            yield return new WaitForSeconds(duration);
            Deactivate();
        }

        private void OnTimelineChangedEventChannel()
        {
            StopAllCoroutines();
            if (ignoreFirstTimelineChanged)
            {
                ignoreFirstTimelineChanged = false;
                return;
            }
            StartCoroutine(Appear());
        }
    }
}