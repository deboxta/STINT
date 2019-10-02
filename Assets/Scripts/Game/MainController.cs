using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class MainController : MonoBehaviour
    {
        private bool first;
        private PlayerDeathEventChannel playerDeathEventChannel;

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            first = true;
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
            if (!SceneManager.GetSceneByName(R.S.Scene.Sebas).isLoaded)
                StartCoroutine(LoadGame());
            else
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(R.S.Scene.Sebas));
        }

        private IEnumerator LoadGame()
        {
            yield return SceneManager.LoadSceneAsync(R.S.Scene.Sebas, LoadSceneMode.Additive);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(R.S.Scene.Sebas));
        }

        private IEnumerator ReloadGame()
        {
            yield return UnloadGame();
            yield return LoadGame();
        }
        
        private IEnumerator UnloadGame()
        {
            yield return SceneManager.UnloadSceneAsync(R.S.Scene.Sebas);
        }
    }
}