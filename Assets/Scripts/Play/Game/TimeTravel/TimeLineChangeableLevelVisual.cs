using Harmony;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public class TimeLineChangeableLevelVisual : MonoBehaviour
    {
        private TimelineChangedEventChannel timelineChangedEventChannel;

        private const float ALPHA_OF_COLOR = 0.35f;
        private TilemapCollider2D tilemapCollider2D;
        private Tilemap tilemap;
        private Color originalColor;

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            tilemap = GetComponent<Tilemap>();
            tilemapCollider2D = GetComponent<TilemapCollider2D>();
            originalColor = tilemap.color;
        }

        private void OnEnable()
        {
            timelineChangedEventChannel.OnTimelineChanged += TimelineChanged;
        }

        private void OnDisable()
        {
            timelineChangedEventChannel.OnTimelineChanged -= TimelineChanged;
        }

        protected void TimelineChanged()
        {
            switch (Finder.TimelineController.CurrentTimeline)
            {
                case Timeline.Primary:
                    if (CompareTag(R.S.Tag.MainTimeline))
                        EnableCollider();
                    else
                        DisableCollider();
                    break;
                case Timeline.Secondary:
                    if (CompareTag(R.S.Tag.SecondaryTimeline))
                        EnableCollider();
                    else
                        DisableCollider();
                    break;
            }
        }

        private void EnableCollider()
        {
            tilemapCollider2D.enabled = true;
            tilemap.color = originalColor;
        }

        private void DisableCollider()
        {
            tilemapCollider2D.enabled = false;
            tilemap.color = new Color(originalColor.r, originalColor.g, originalColor.b, ALPHA_OF_COLOR);
        }
    }
}