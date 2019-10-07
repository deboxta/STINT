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
        private string currentScene;
        private string sceneToLoad;

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
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
        }

        private IEnumerator ReloadGame()
        {
            Finder.Player.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            yield return UnloadGame();
            yield return LoadGame();
            Finder.TimeController.ResetTimeline();
        }
        
        private IEnumerator UnloadGame()
        {
            yield return SceneManager.UnloadSceneAsync(currentScene);
        }
    }
}