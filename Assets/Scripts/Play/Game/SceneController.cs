using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    //Author : Sébastien Arsenault
    [Findable(R.S.Tag.MainController)]
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private int startingScene = 0;
        [SerializeField] private float waitingTime = 1.5f;
        
        private PlayerDeathEventChannel playerDeathEventChannel;
        private LevelCompletedEventChannel levelCompletedEventChannel;
        private SavedDataLoadedEventChannel savedDataLoadedEventChannel;
        private SavedSceneLoadedEventChannel savedSceneLoadedEventChannel;
        private NewGameLoadedEventChannel newGameLoadedEventChannel;
        private SaveSystem saveSystem;
        private Scenes scenes;
        private int currentScene;
        private Dispatcher dispatcher;

        public int CurrentScene => currentScene;

        private void Awake()
        {
            dispatcher = Finder.Dispatcher;
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
            savedDataLoadedEventChannel = Finder.SavedDataLoadedEventChannel;
            savedSceneLoadedEventChannel = Finder.SavedSceneLoadedEventChannel;
            newGameLoadedEventChannel = Finder.NewGameLoadedEventChannel;
            saveSystem = Finder.SaveSystem;
            scenes = GetComponentInChildren<Scenes>();
            
            currentScene = startingScene;
        }
        
        private void Start()
        {
            if (!SceneManager.GetSceneByName(scenes.GetSceneName(currentScene)).isLoaded)
                StartCoroutine(LoadGame());
            else
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenes.GetSceneName(currentScene)));
        }
        
        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += OnPlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted += OnLevelCompleted;
            savedDataLoadedEventChannel.OnSavedDataLoaded += OnSavedDataLoaded;
        }

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= OnPlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted -= OnLevelCompleted;
            savedDataLoadedEventChannel.OnSavedDataLoaded -= OnSavedDataLoaded;
        }

        private void OnPlayerDeath()
        {
            StartCoroutine(RestartLevel());
        }
        
        private void OnLevelCompleted()
        {
            StartCoroutine(NextLevel());
        }
        //By Yannick Cote
        private void OnSavedDataLoaded()
        {
            StartCoroutine(LoadSavedScene());
        }
        //By Yannick Cote
        private IEnumerator LoadSavedScene()
        {
            yield return UnloadGame();
            currentScene = dispatcher.DataCollector.ActiveScene;
            yield return LoadGame();
            savedSceneLoadedEventChannel.NotifySavedDataLoaded();
        }

        private IEnumerator NextLevel()
        {
            yield return new WaitForSeconds(waitingTime);
            yield return UnloadGame();
            
            if (currentScene == 0)
                newGameLoadedEventChannel.NotifyNewGameLoaded();

            currentScene++;

            yield return LoadGame();

        }

        private IEnumerator LoadGame()
        {
            yield return SceneManager.LoadSceneAsync(scenes.GetSceneName(currentScene), LoadSceneMode.Additive);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenes.GetSceneName(currentScene)));
        }

        private IEnumerator UnloadGame()
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(scenes.GetSceneName(currentScene)));
        }

        private IEnumerator RestartLevel()
        {
            yield return new WaitForSeconds(waitingTime);
            yield return UnloadGame();
            //By Yannick Cote
            if (dispatcher.DataCollector.ActiveScene != null)
            {
                currentLevel = dispatcher.DataCollector.ActiveScene.Value;
                yield return LoadGame();
                savedSceneLoadedEventChannel.NotifySavedDataLoaded();
            }
            else
            {
                yield return LoadGame();
            }
        }

        //By Yannick Cote
        public void ReturnToMainMenu()
        {
            StartCoroutine(MenuReturn());
        }

        //By Yannick Cote
        private IEnumerator MenuReturn()
        {
            yield return UnloadGame();
            currentScene = 0;
            yield return LoadGame();
        }
    }
}
