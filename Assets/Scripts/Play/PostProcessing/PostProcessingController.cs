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
        
        private DepthOfField depthOfField;
        private ChromaticAberration chromaticAberration;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private LevelCompletedEventChannel levelCompletedEventChannel;
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private TimelineController timelineController;
        private TimeFreezeEventChannel timeFreezeEventChannel;
        private TimeFreezeWarningEventChannel timeFreezeWarningEventChannel;
        private TimeFreezeController timeFreezeController;
        private CinemachinePostProcessing volume;

        private void Awake()
        {
            volume = gameObject.GetComponent<CinemachinePostProcessing>();

            volume.m_Profile = volume.Profile.Clone();
            
            volume.Profile.TryGetSettings(out depthOfField);
            volume.Profile.TryGetSettings(out chromaticAberration);

            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
            timelineController = Finder.TimelineController;
            timeFreezeEventChannel = Finder.TimeFreezeEventChannel;
            timeFreezeWarningEventChannel = Finder.TimeFreezeWarningEventChannel;
            timeFreezeController = Finder.TimeFreezeController;
        }

        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += OnPlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted += OnLevelCompleted;
            timelineChangedEventChannel.OnTimelineChanged += OnTimeLineChanged;
            timeFreezeEventChannel.OnTimeFreezeStateChanged += OnTimeFreezeChanged;
            timeFreezeWarningEventChannel.OnTimeFreezeWarning += OnTimeFreezeWarning;
        }

        

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= OnPlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted -= OnLevelCompleted;
            timelineChangedEventChannel.OnTimelineChanged -= OnTimeLineChanged;
            timeFreezeEventChannel.OnTimeFreezeStateChanged -= OnTimeFreezeChanged;
            timeFreezeWarningEventChannel.OnTimeFreezeWarning -= OnTimeFreezeWarning;
        }

        private void OnTimeLineChanged()
        {
            if (timelineController.CurrentTimeline == Timeline.Primary)
            {
                volume.m_Profile.GetSetting<ColorGrading>().active = false;
                volume.m_Profile.GetSetting<LensDistortion>().active = false;
                volume.m_Profile.GetSetting<Bloom>().active = false;
                volume.m_Profile.GetSetting<Grain>().active = false;
            }
            else
            {
                volume.m_Profile.GetSetting<ColorGrading>().active = true;
                volume.m_Profile.GetSetting<LensDistortion>().active = true;
                volume.m_Profile.GetSetting<Bloom>().active = true;
                volume.m_Profile.GetSetting<Grain>().active = true;
            }
        }

        private void OnLevelCompleted()
        {
            StopAllCoroutines();
            StartCoroutine(FocusOut());
        }

        private void OnPlayerDeath()
        {
            StopAllCoroutines();
            StartCoroutine(FocusOut());
        }

        private void OnTimeFreezeChanged()
        {
            if (timeFreezeController.IsFrozen)
            {
                chromaticAberration.intensity.value = 1;
            }
            else
            {
                chromaticAberration.intensity.value = 0;
            }
        }

        private void OnTimeFreezeWarning()
        {
            if (timeFreezeController.IsFrozen)
            {
                StopAllCoroutines();
                StartCoroutine(ChromaticWarningOff());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(ChromaticWarningOn());
            }
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

        private IEnumerator ChromaticWarningOn()
        {
            if (chromaticAberration.intensity.value < 1f)
            {
                chromaticAberration.intensity.value += 0.05f;
                yield return new WaitForSeconds(0.025f);
                yield return ChromaticWarningOn();
            }
        }

        private IEnumerator ChromaticWarningOff()
        {
            if (chromaticAberration.intensity.value > 0f)
            {
                chromaticAberration.intensity.value -= 0.05f;
                yield return new WaitForSeconds(0.025f);
                yield return ChromaticWarningOff();
            }
        }
    }
}