using System.Collections;
using Cinemachine.PostFX;
using Harmony;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

namespace Game
{
    public class PostProcessingController : MonoBehaviour
    {
        [SerializeField] private float fadeSpeed = 0.025f;
        [SerializeField] private float fadeBy = 0.1f;
        
        private ColorGrading colorGrading;
        private DepthOfField depthOfField;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private LevelCompletedEventChannel levelCompletedEventChannel;
        private CinemachinePostProcessing volume;

        private void Awake()
        {
            volume = gameObject.GetComponent<CinemachinePostProcessing>();

            volume.m_Profile = volume.Profile.Clone();

            volume.Profile.TryGetSettings(out colorGrading);
            volume.Profile.TryGetSettings(out depthOfField);

            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
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
            //StartCoroutine(FocusOut());
        }

        private void OnPlayerDeath()
        {
            StartCoroutine(FocusOut());
        }

        private IEnumerator FocusOut()
        {
            depthOfField.focusDistance.value -= fadeBy;
            yield  return new WaitForSeconds(fadeSpeed);
            if (depthOfField.focusDistance.value >= 1)
            {
                yield return FocusOut();
            }
        }

        private IEnumerator FocusIn()
        {
            depthOfField.focusDistance.value += fadeBy;
            yield  return new WaitForSeconds(fadeSpeed);
            if (depthOfField.focusDistance.value <= 5)
            {
                yield return FocusIn();
            }
        }
    }
}