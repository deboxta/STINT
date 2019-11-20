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
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private TimelineController timelineController;
        private CinemachinePostProcessing volume;

        private void Awake()
        {
            volume = gameObject.GetComponent<CinemachinePostProcessing>();

            volume.m_Profile = volume.Profile.Clone();

            volume.Profile.TryGetSettings(out colorGrading);
            volume.Profile.TryGetSettings(out depthOfField);

            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            timelineController = Finder.TimelineController;
        }

        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += OnPlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted += OnLevelCompleted;
            timelineChangedEventChannel.OnTimelineChanged += OnTimeLineChanged;
        }

        

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= OnPlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted -= OnLevelCompleted;
            timelineChangedEventChannel.OnTimelineChanged -= OnTimeLineChanged;
        }

        private void OnTimeLineChanged()
        {
            if (timelineController.CurrentTimeline == Timeline.Primary)
            {
                volume.enabled = false;
            }
            else
            {
                volume.enabled = true;
            }
            
            //volume.enabled = !volume.enabled;
            //colorGrading.enabled.value = !colorGrading.enabled.value;
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