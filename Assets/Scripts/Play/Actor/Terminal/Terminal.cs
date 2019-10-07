using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class Terminal : MonoBehaviour
    {
        [SerializeField] private Sprite spriteDenied;
        [SerializeField] private Sprite spriteOpen;
        
        private LevelCompletedEventChannel levelCompletedEventChannel;
        private SpriteRenderer spriteRenderer;
        private ISensor<Player> playerSensor;

        private void Awake()
        {
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
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

        private void OnPlayerUnSensed(Player otherobject)
        {
            spriteRenderer.sprite = spriteDenied;
        }

        private void OnPlayerSensed(Player otherObject)
        {
            spriteRenderer.sprite = spriteOpen;
        }
    }
}