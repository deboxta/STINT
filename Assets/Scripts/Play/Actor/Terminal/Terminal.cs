using UnityEngine;

namespace Game
{
    public class Terminal : MonoBehaviour
    {
        [SerializeField] protected Sprite spriteDenied;
        [SerializeField] protected Sprite spriteOpen;

        private SpriteRenderer spriteRenderer;
        private ISensor<Player> playerSensor;

        protected virtual void Awake()
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

        protected virtual void OnPlayerSensed(Player player)
        {
            spriteRenderer.sprite = spriteOpen;
        }
    }
}