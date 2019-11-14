using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    //Author : Sébastien Arsenault
    //BC : Pourquoi tout ça n'est pas dans le "MainController" ?
    //     En fait, quelle est la distinction que vous voulez faire entre "LevelController"
    //     et le "MainController" ? Est-ce que cette distinction est claire pour tous les membres
    //     de l'équipe, et est-ce qu'elle est représentée dans le cas. Actuellement, j'en doute.
    //
    //     Je crois que ce que vous avez tenté de faire, c'est un "LevelController" par niveau.
    //     Lorsque le joueur meurt, le niveau veut faire ses propres choses (tel que montrer
    //     un texte disant "Vous êtes mort") avant d'avertir le "MainController" de recharger
    //     le niveau tout quelque chose du genre.
    //
    //     Bref, je vous en ai parlé sur Discord de toute façon.
    [Findable(R.S.Tag.MainController)]
    public class LevelController : MonoBehaviour
    {
        //BC : Devrait être un "SerializedField".
        private const int STARTING_LEVEL = 0;
        
        private PlayerDeathEventChannel playerDeathEventChannel;
        private LevelCompletedEventChannel levelCompletedEventChannel;
        private LevelScenes levelScenes;
        private int currentLevel;
        
        public int CurrentLevel
        {
            get => currentLevel;
        }

        private void Awake()
        {
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
            levelScenes = GetComponentInChildren<LevelScenes>();
            
            currentLevel = STARTING_LEVEL;
        }
        
        private void Start()
        {
            //BR : SVP, évitez de mélanger les styles de "{".
            //     Soit vous en mettez pour la série de conditions, soit vous en mettez pas.
            //     Mais ne faites pas une condition sans "{" et une autre avec.
            //BC : "currentLevel" est le "SceneIndex" n'est ce pas ?
            //     Il est possible d'obtenir directement une scene par son index.
            //     Voir SceneManager.GetSceneAt().
            //
            //     De plus, je ferais attention avec ça. Le "SceneIndex" est dépendant de ce que vous avez
            //     choisi dans les "BuildSettings". 
            if (!SceneManager.GetSceneByName(levelScenes.GetSceneName(currentLevel)).isLoaded)
                StartCoroutine(LoadGame());
            else
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelScenes.GetSceneName(currentLevel)));
            }
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

        //BR : Par convention, si c'est une méthode qui est enregistré à un événement, on la préfixe
        //     par "On".
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

        private IEnumerator LoadGame()
        {
            //BR : Voir "SceneManager.GetSceneAt".
            yield return SceneManager.LoadSceneAsync(levelScenes.GetSceneName(currentLevel), LoadSceneMode.Additive);

            //BR : Voir "SceneManager.GetSceneAt".
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelScenes.GetSceneName(currentLevel)));
            
            Finder.TimelineController.ResetTimeline();
        }

        private IEnumerator UnloadGame()
        {
            //BR : Voir "SceneManager.GetSceneAt".
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(levelScenes.GetSceneName(currentLevel)));
        }

        private IEnumerator RestartLevel()
        {
            //BC : Cela me semble être une "Patch".
            //     Aussi, chiffre magique.
            Finder.Player.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            yield return UnloadGame();
            yield return LoadGame();
        }
        
        //By Yannick Cote
        //BC : Je vois pas le lien entre "Level" et "Menu".
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
            Finder.TimelineController.ResetTimeline();
            Finder.TimeFreezeController.Reset();
        }
    }
}
