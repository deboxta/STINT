﻿using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class TerminalV2 : MonoBehaviour
    {
        [SerializeField] protected Sprite spriteDenied;
        [SerializeField] protected Sprite spriteOpen;

        private SpriteRenderer spriteRenderer;
        private ISensorV2<Player> playerSensor;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            playerSensor = GetComponent<SensorV2>().For<Player>();

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