using System;
using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class MainController : MonoBehaviour
    {
        [SerializeField] private string sceneNameToLoad = "Game";
        [SerializeField] private string nextSceneToLoad;
        private PlayerDeathEventChannel playerDeathEventChannel;
        //TODO check if this is really needed
        private PlayerHitEventChannel playerHitEventChannel;
        private LevelCompletedEventChannel levelCompletedEventChannel;
        private string currentScene;
        private string sceneToLoad;

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
            playerHitEventChannel = Finder.PlayerHitEventChannel;
            currentScene = SceneManager.GetActiveScene().name;
            sceneToLoad = sceneNameToLoad;
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
            StartCoroutine(ReloadGame());
        }

        private void LevelCompleted()
        {
            sceneToLoad = nextSceneToLoad;
            StartCoroutine(ReloadGame());
        }

        private void Start()
        {
            if (!SceneManager.GetSceneByName(sceneToLoad).isLoaded)
                StartCoroutine(LoadGame());
            else
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
                currentScene = sceneToLoad;
            }
        }

        private IEnumerator LoadGame()
        {
            yield return SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
            currentScene = sceneToLoad;
        }

        private IEnumerator ReloadGame()
        {
            yield return UnloadGame();
            yield return LoadGame();
        }
        
        private IEnumerator UnloadGame()
        {
            yield return SceneManager.UnloadSceneAsync(currentScene);
        }
    }
}