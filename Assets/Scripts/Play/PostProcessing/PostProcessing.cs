using System;
using System.Collections;
using Cinemachine.PostFX;
using Harmony;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Game
{
    public class PostProcessing : MonoBehaviour
    {
        [SerializeField] private float fadeSpeed = 0.0001f;
        [SerializeField] private float fadeBy = 0.25f;
        
        private ColorGrading colorGrading;
        private DepthOfField depthOfField;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private LevelCompletedEventChannel levelCompletedEventChannel;
        private CinemachinePostProcessing volume;

        private void Awake()
        {
            volume = gameObject.GetComponent<CinemachinePostProcessing>();

            volume.m_Profile.TryGetSettings(out colorGrading);
            volume.m_Profile.TryGetSettings(out depthOfField);

            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
        }

        private void OnDestroy()
        {
            depthOfField.focusDistance.value = 5;
        }

        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += OnPlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted += OnLevelCompleted;
        }

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= OnPlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted -= OnLevelCompleted;
        }
        
        private void OnLevelCompleted()
        {
            StartCoroutine(FadeOut());
        }

        private void OnPlayerDeath()
        {
            StartCoroutine(FadeOut());        
        }

        private IEnumerator FadeOut()
        {
            depthOfField.focusDistance.value -= fadeBy;
            volume.InvalidateCachedProfile();
            yield  return new WaitForSeconds(fadeSpeed);
            if (depthOfField.focusDistance.value >= 1)
            {
                yield return FadeOut();
            }
        }
        
        private IEnumerator FadeIn()
        {
            depthOfField.focusDistance.value += fadeBy;
            volume.InvalidateCachedProfile();
            yield  return new WaitForSeconds(fadeSpeed);
            if (depthOfField.focusDistance.value <= 1)
            {
                yield return FadeOut();
            }
        }
    }
}