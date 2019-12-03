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
        private Color ghostColor;

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            tilemap = GetComponent<Tilemap>();
            tilemapCollider2D = GetComponent<TilemapCollider2D>();
            originalColor = tilemap.color;
            ghostColor = new Color(originalColor.r, originalColor.g, originalColor.b, ALPHA_OF_COLOR);
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
            DisableDeathZoneColliders(true);
        }

        private void DisableCollider()
        {
            tilemapCollider2D.enabled = false;
            tilemap.color = ghostColor;
            DisableDeathZoneColliders(false);
        }

        //disable colliders responsible for the player death when he change timeline in wall position.
        private void DisableDeathZoneColliders(bool active)
        {
            GameObject[] childrens = this.Children();

            foreach (var children in childrens)
            {
                if (children.CompareTag(R.S.Tag.DeathZone))
                {
                    children.SetActive(active);
                }
            }
        }
    }
}