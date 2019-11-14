using Harmony;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

//Author : Yannick Cote
namespace Game
{
    [Findable(R.S.Tag.MenuController)]
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject[] menuPages = null;
        [SerializeField] private GameObject popupWindow = null;
        
        private MenuPageChangedEventChannel menuPageChangedEventChannel;
        private LevelCompletedEventChannel levelCompletedEventChannel;
        
        private GameObject activePage;
        private SaveSystem saveSystem;
        private Dispatcher dispatcher;

        public GameObject ActivePage => activePage;

        private void Awake()
        {
            menuPageChangedEventChannel = Finder.MenuPageChangedEventChannel;
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;

            dispatcher = Finder.Dispatcher;
            saveSystem = new SaveSystem();
        }
        
        private GameObject GetActivePage()
        {
            for (int i = 0; i < menuPages.Length; i++)
            {
                if (menuPages[i].activeSelf == true)
                {
                    return menuPages[i];
                }            
            }

            return null;
        }

        private void Update()
        {
            activePage = GetActivePage();
        }

        [UsedImplicitly]
        public void StartGame(GameObject fileName)
        {
            saveSystem.LoadData(fileName.GetComponent<Text>().text);
        }

        [UsedImplicitly]
        public void SetPage(GameObject toEnable)
        {
            activePage.SetActive(false);
            toEnable.SetActive(true);
            menuPageChangedEventChannel.NotifyPageChanged();
        }
        
        [UsedImplicitly]
        public void CreateNewGameFile(InputField fileName)
        {
            if (fileName.text != null)
            {
                dispatcher.DataCollector.Name = fileName.text;
                levelCompletedEventChannel.NotifyLevelCompleted();
            }
        }
        
        [UsedImplicitly]
        public void SaveGameStatus()
        {
            saveSystem.SaveGame();
        }
        
        [UsedImplicitly]
        public void ClosePopup()
        {
            popupWindow.SetActive(false);
        }
        
        [UsedImplicitly]
        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}