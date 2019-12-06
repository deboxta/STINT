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
        //private TilemapRenderer tileMapRenderer;
        private Tilemap tilemap;
        private Rigidbody2D rigidBody2D;
        private TilemapCollider2D tileMapCollider2D;
        
        private Color originalColor;
        private Color ghostColor;

        private bool ignoreFirstTimelineChanged;

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            //tileMapRenderer = GetComponent<TilemapRenderer>();
            tilemap = GetComponent<Tilemap>();
            rigidBody2D = GetComponent<Rigidbody2D>();
            tileMapCollider2D = GetComponent<TilemapCollider2D>();
            
            //originalColor = tileMapRenderer.color;
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
            //tileMapRenderer.enabled = false;
            tilemap.enabled = false;
            rigidBody2D.simulated = false;
            tileMapCollider2D.enabled = false;
            tilemap.color = ghostColor;
        }
        
        private void Activate()
        {
            //tileMapRenderer.enabled = true;
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