using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    //Author : Sébastien Arsenault
    public class TemporaryPlatform : MonoBehaviour
    {
        private const float ALPHA_OF_COLOR = 0.35f;
        
        [SerializeField] [Range(1, 20)] private float duration = 5;
        
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private Tilemap tilemap;
        private Rigidbody2D rigidBody2D;
        private TilemapCollider2D tileMapCollider2D;
        
        private Color originalColor;
        private Color ghostColor;

        private bool ignoreFirstTimelineChanged;

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            tilemap = GetComponent<Tilemap>();
            rigidBody2D = GetComponent<Rigidbody2D>();
            tileMapCollider2D = GetComponent<TilemapCollider2D>();
            
            originalColor = tilemap.color;
            ghostColor = new Color(originalColor.r, originalColor.g, originalColor.b, ALPHA_OF_COLOR);
            
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
            tilemap.enabled = false;
            rigidBody2D.simulated = false;
            tileMapCollider2D.enabled = false;
            tilemap.color = ghostColor;
        }
        
        private void Activate()
        {
            tilemap.enabled = true;
            rigidBody2D.simulated = true;
            tileMapCollider2D.enabled = true;
            tilemap.color = originalColor;
            
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