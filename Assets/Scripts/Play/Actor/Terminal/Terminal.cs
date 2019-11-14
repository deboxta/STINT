using System;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    //BR : Devrait être abtraite.
    public class Terminal : MonoBehaviour
    {
        [SerializeField] protected Sprite spriteDenied;
        [SerializeField] protected Sprite spriteOpen;

        private SpriteRenderer spriteRenderer;
        private ISensor<Player> playerSensor;

        //BR : Avec Unity, pas besoin que cette méthode soit abstraite.
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

        //BR : Pourait utiliser le patron "Template Method".
        //     J'ai mis en bas l'implémentation pour que vous voyez de quoi je veux parler.
        //     J'en ai aussi profité pour faire du découpage.
        protected virtual void OnPlayerSensed(Player player)
        {
            spriteRenderer.sprite = spriteOpen;
        }
    }
    
    public abstract class Terminal2 : MonoBehaviour
    {
        [SerializeField] protected Sprite spriteOpen;
        [SerializeField] protected Sprite spriteDenied;

        private SpriteRenderer spriteRenderer;
        private ISensor<Player> playerSensor;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerSensor = GetComponent<Sensor>().For<Player>();
        }

        protected void Start()
        {
            ShowDenied();
        }

        private void OnEnable()
        {
            playerSensor.OnSensedObject += OnPlayerSensedInternal;
            playerSensor.OnUnsensedObject += OnPlayerUnSensedInternal;
        }

        private void OnDisable()
        {
            playerSensor.OnSensedObject -= OnPlayerSensed;
            playerSensor.OnUnsensedObject -= OnPlayerUnSensedInternal;
        }

        protected virtual void OnPlayerSensedInternal(Player player)
        {
            ShowOpened();
            OnPlayerSensed(player);
        }

        private void OnPlayerUnSensedInternal(Player player)
        {
            ShowDenied();
            OnPlayerUnSensed(player);
        }

        protected virtual void OnPlayerSensed(Player player)
        {
            //Empty on purpose. Implementor can remplace this with something else.
        }

        protected virtual void OnPlayerUnSensed(Player player)
        {
            //Empty on purpose. Implementor can remplace this with something else.
        }

        private void ShowOpened()
        {
            spriteRenderer.sprite = spriteOpen;
        }
        
        private void ShowDenied()
        {
            spriteRenderer.sprite = spriteDenied;
        }
    }
}