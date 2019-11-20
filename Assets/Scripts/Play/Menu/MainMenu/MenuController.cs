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
        [SerializeField] private GameObject firstPage = null;
        [SerializeField] private GameObject popupWindow = null;
        
        private MenuPageChangedEventChannel menuPageChangedEventChannel;
        private LevelCompletedEventChannel levelCompletedEventChannel;
        
        private GameObject activePage;
        private SaveSystem saveSystem;
        private Dispatcher dispatcher;
        private GameObject saveToDelete;

        public GameObject ActivePage => activePage;
        
        private void Awake()
        {
            menuPageChangedEventChannel = Finder.MenuPageChangedEventChannel;
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;

            dispatcher = Finder.Dispatcher;
            saveSystem = Finder.SaveSystem;
            activePage = firstPage;
        }

        [UsedImplicitly]
        public void DeleteSave()
        {
            if (saveToDelete != null)
            {
                saveSystem.DeleteSave(saveToDelete.GetComponent<Text>().text);
            }
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
            activePage = toEnable;
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
        public void OpenPopup(GameObject saveToDelete)
        {
            this.saveToDelete = saveToDelete;
            popupWindow.SetActive(true);
        }
        
        //TODO: Should be called in maincontroller when maincontroller is implemented
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