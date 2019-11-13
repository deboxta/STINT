using System;
using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class FadeInAndOut : MonoBehaviour
    {
        [SerializeField] private float fadeSpeed = 0.025f;
        [SerializeField] private float fadeBy = 0.1f;

        private PlayerDeathEventChannel playerDeathEventChannel;
        private Image image;

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += OnPlayerDeath;
            SceneManager.sceneLoaded += OnSceneLoaded;
            //SceneManager.sceneUnloaded += OnSceneUnLoaded;
        }

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= OnPlayerDeath;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            //SceneManager.sceneUnloaded -= OnSceneUnLoaded;
        }

        private void OnPlayerDeath()
        {
            StartCoroutine(FadeOut());
        }
        
        private void OnSceneUnLoaded(Scene arg0)
        {
            StartCoroutine(FadeOut());
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            var imageColor = image.color;
            
            imageColor[3] = imageColor[3] - fadeBy ;
            image.color = imageColor;
            if (imageColor.a >= 0)
            {
                yield return new WaitForSeconds(fadeSpeed);
                yield return FadeIn();
            }
        }

        private IEnumerator FadeOut()
        {
            var imageColor = image.color;
            
            imageColor[3] = fadeBy + imageColor[3];
            image.color = imageColor;
            if (imageColor.a <= 1)
            {
                yield return new WaitForSeconds(fadeSpeed);
                yield return FadeOut();
            }
        }
    }
}