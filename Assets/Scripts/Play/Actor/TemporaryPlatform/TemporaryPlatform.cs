using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    //BR : Globalement clean.
    
    //Author : Sébastien Arsenault
    public class TemporaryPlatform : MonoBehaviour
    {
        [SerializeField] [Range(1, 20)] private float duration = 5;
        
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private TilemapRenderer tileMapRenderer;
        private Rigidbody2D rigidBody2D;
        private CompositeCollider2D compositeCollider2D;
        private TilemapCollider2D tileMapCollider2D;

        private bool ignoreFirstTimeLineChanged;

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            //BR : Une Tilemap par plateforme ? C'est pas un peu "Overkill" ?
            //     Cela ne risque pas de créer des gros problèmes un jour ?
            tileMapRenderer = GetComponent<TilemapRenderer>();
            rigidBody2D = GetComponent<Rigidbody2D>();
            compositeCollider2D = GetComponent<CompositeCollider2D>();
            tileMapCollider2D = GetComponent<TilemapCollider2D>();
            
            Deactivate();

            ignoreFirstTimeLineChanged = true;
        }

        private void OnEnable()
        {
            //BR : Appelle "Deactivate" ici. Je dis cela pour éviter un bogue dans le futur.
            //     Me voir si c'est pas clair du "pourquoi".
            timelineChangedEventChannel.OnTimelineChanged += OnTimelineChangedEventChannel;
        }
        
        private void OnDisable()
        {
            timelineChangedEventChannel.OnTimelineChanged -= OnTimelineChangedEventChannel;
            //BR : Appelle "Deactivate" ici aussi.
        }

        private void Deactivate()
        {
            tileMapRenderer.enabled = false;
            rigidBody2D.simulated = false;
            //BR : ??? Pourquoi tu "active" ces colliders ?
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

        //BR : Clean. C'est exactement ce que j'attends d'une coroutine.
        private IEnumerator Appear()
        {
            Activate();
            yield  return new WaitForSeconds(duration);
            Deactivate();
        }

        private void OnTimelineChangedEventChannel()
        {
            //BR : C'est vraiment nécessaire ça ? J'ai un peu une idée du pourquoi,
            //     mais cela ressemble vraiment à une patch (mais comme j'ai pas de preuve sous la main,
            //     mettons que j'ai rien vu...)
            if (ignoreFirstTimeLineChanged)
            {
                ignoreFirstTimeLineChanged = false;
                return;
            }
            StartCoroutine(Appear());
        }
    }
}