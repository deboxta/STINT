using UnityEngine;

namespace Game
{
    public class TerminalDialogue : MonoBehaviour
    {
        [SerializeField] private Sprite spriteDenied;
        [SerializeField] private Sprite spriteOpen;
        
        private SpriteRenderer spriteRenderer;
        private ISensor<Player> playerSensor;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerSensor = GetComponent<Sensor>().For<Player>();

            spriteRenderer.sprite = spriteDenied;
        }
        
        private void OnEnable()
        {
            playerSensor.OnSensedObject += OnPlayerSensed;
            playerSensor.OnUnsensedObject += OnPlayerUnSensed;
        }

        private void OnDisable()
        {
            playerSensor.OnSensedObject -= OnPlayerSensed;
            playerSensor.OnUnsensedObject -= OnPlayerUnSensed;
        }

        private void OnPlayerUnSensed(Player player)
        {
            spriteRenderer.sprite = spriteDenied;
        }

        private void OnPlayerSensed(Player player)
        {
            spriteRenderer.sprite = spriteOpen;
        }
    }
}