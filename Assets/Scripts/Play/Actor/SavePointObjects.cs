using System;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Yannick Cote
    public class SavePointObjects : MonoBehaviour
    {
        [Header("Images")] 
        [SerializeField] private Sprite notSavedSprite;
        [SerializeField] private Sprite savedSprite;
        
        private SaveSystem saveSystem;
        private Dispatcher dispatcher;
        private SpriteRenderer spriteRenderer;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private bool isGameSaved;

        private bool isRaycastTriggered;
        private int playerLayer;

        private RaycastHit2D rayDown;
        private RaycastHit2D rayUp;

        private void Awake()
        {
            dispatcher = Finder.Dispatcher;
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            saveSystem = Finder.SaveSystem;
            spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = notSavedSprite;
            isGameSaved = false;
            isRaycastTriggered = false;
            playerLayer = (1 << LayerMask.NameToLayer(R.S.Layer.Player));

        }

        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += PlayerDeath;
        }

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= PlayerDeath;
        }

        private void PlayerDeath()
        {
            if (isGameSaved)
            {
                saveSystem.LoadData(dispatcher.DataCollector.Name);
            }
        }
        

        private void Update()
        {
            rayUp = Physics2D.Raycast(transform.position, Vector2.up,30 , playerLayer);
            rayDown = Physics2D.Raycast(transform.position, Vector2.down, 30, playerLayer);
            if (rayUp)
                isRaycastTriggered = true;
            else if (rayDown)
                isRaycastTriggered = true;
            
            if (isRaycastTriggered && !isGameSaved)
            {
                saveSystem.SaveGame();
                isGameSaved = true;
            }
            
            if (isGameSaved)
            {
                spriteRenderer.sprite = savedSprite;
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, rayUp.point);
            Gizmos.DrawLine(transform.position, rayDown.point);
        }
#endif
    }
}