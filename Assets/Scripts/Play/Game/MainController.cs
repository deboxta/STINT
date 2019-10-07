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
        private PlayerDeathEventChannel playerDeathEventChannel;
        //TODO check if this is really needed
        private PlayerHitEventChannel playerHitEventChannel;
        private string currentScene;
        private string sceneToLoad;

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            playerHitEventChannel = Finder.PlayerHitEventChannel;
            currentScene = SceneManager.GetActiveScene().name;
            sceneToLoad = sceneNameToLoad;
        }

        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += PlayerDeath;
        }

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= PlayerDeath;
        }

        private void PlayerDeath()
        {
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
            Finder.TimelineController.CurrentTimeline = Timeline.Main;
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