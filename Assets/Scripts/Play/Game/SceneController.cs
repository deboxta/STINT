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
        private const int STARTING_LEVEL = 0;
        
        private PlayerDeathEventChannel playerDeathEventChannel;
        private LevelCompletedEventChannel levelCompletedEventChannel;
        private SavedDataLoadedEventChannel savedDataLoadedEventChannel;
        private SavedSceneLoadedEventChannel savedSceneLoadedEventChannel;
        private NewGameLoadedEventChannel newGameLoadedEventChannel;
        private SaveSystem saveSystem;
        private Scenes scenes;
        private int currentLevel;
        private Dispatcher dispatcher;

        public int CurrentLevel
        {
            get => currentLevel;
        }

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
            
            currentLevel = STARTING_LEVEL;
        }
        
        private void Start()
        {
            if (!SceneManager.GetSceneByName(scenes.GetSceneName(currentLevel)).isLoaded)
                StartCoroutine(LoadGame());
            else
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenes.GetSceneName(currentLevel)));
            }
        }
        
        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += PlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted += LevelCompleted;
            savedDataLoadedEventChannel.OnSavedDataLoaded += SavedDataLoaded;
        }

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= PlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted -= LevelCompleted;
            savedDataLoadedEventChannel.OnSavedDataLoaded -= SavedDataLoaded;
        }

        private void PlayerDeath()
        {
            StartCoroutine(RestartLevel());
        }
        
        private void LevelCompleted()
        {
            StartCoroutine(NextLevel());
        }
        //By Yannick Cote
        private void SavedDataLoaded()
        {
            StartCoroutine(LoadSavedScene());
        }
        //By Yannick Cote
        private IEnumerator LoadSavedScene()
        {
            yield return UnloadGame();
            //ActiveScene is never null when this function is called
            currentLevel = dispatcher.DataCollector.ActiveScene.Value;
            yield return LoadGame();
            savedSceneLoadedEventChannel.NotifySavedDataLoaded();
        }

        private IEnumerator NextLevel()
        {
            yield return new WaitForSeconds(1.5f);
            yield return UnloadGame();
            
            if (currentLevel == 0)
            {
                newGameLoadedEventChannel.NotifyNewGameLoaded();
            }

            currentLevel++;

            yield return LoadGame();

        }

        private IEnumerator LoadGame()
        {
            yield return SceneManager.LoadSceneAsync(scenes.GetSceneName(currentLevel), LoadSceneMode.Additive);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenes.GetSceneName(currentLevel)));
        }

        private IEnumerator UnloadGame()
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(scenes.GetSceneName(currentLevel)));
        }

        private IEnumerator RestartLevel()
        {
            yield return new WaitForSeconds(1.5f);
            yield return UnloadGame();
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
            currentLevel = 0;
            yield return LoadGame();
        }
    }
}
