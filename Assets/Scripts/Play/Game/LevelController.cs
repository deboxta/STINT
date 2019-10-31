using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class LevelController : MonoBehaviour
    {
        private const int STARTING_LEVEL = 0;
        
        private PlayerDeathEventChannel playerDeathEventChannel;
        private LevelCompletedEventChannel levelCompletedEventChannel;
        private LevelScenes levelScenes;
        private int currentLevel;

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
            levelScenes = GetComponentInChildren<LevelScenes>();
            
            currentLevel = STARTING_LEVEL;
        }
        
        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += PlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted += LevelCompleted;
        }

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= PlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted -= LevelCompleted;
        }

        private void PlayerDeath()
        {
            StartCoroutine(RestartLevel());
        }

        private void LevelCompleted()
        {
            StartCoroutine(NextLevel());
        }

        private IEnumerator NextLevel()
        {
            yield return UnloadGame();
            currentLevel++;
            yield return LoadGame();
        }

        private void Start()
        {
            if (!SceneManager.GetSceneByName(levelScenes.GetSceneName(currentLevel)).isLoaded)
                StartCoroutine(LoadGame());
            else
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelScenes.GetSceneName(currentLevel)));
            }
        }

        private IEnumerator LoadGame()
        {
            yield return SceneManager.LoadSceneAsync(levelScenes.GetSceneName(currentLevel), LoadSceneMode.Additive);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelScenes.GetSceneName(currentLevel)));
        }

        private IEnumerator RestartLevel()
        {
            Finder.Player.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            yield return UnloadGame();
            yield return LoadGame();
            Finder.TimelineController.ResetTimeline();
        }
        
        private IEnumerator UnloadGame()
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(levelScenes.GetSceneName(currentLevel)));
        }
    }
}