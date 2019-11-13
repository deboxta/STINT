using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class FadeInAndOut : MonoBehaviour
    {
        [SerializeField] private float fadeSpeed = 0.1f;
        
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnLoaded;
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
            
            imageColor[3] -= fadeSpeed;
            image.color = imageColor;
            if (imageColor.a >= 0)
            {
                yield  return new WaitForSeconds(0.025f);
                yield return FadeIn();
            }
        }

        private IEnumerator FadeOut()
        {
            var imageColor = image.color;
            
            imageColor[3] = fadeSpeed + imageColor[3];
            image.color = imageColor;
            if (imageColor.a <= 1)
            {
                yield  return new WaitForSeconds(0.025f);
                yield return FadeOut();
            }
        }
    }
}