using System;
using Harmony;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Button = UnityEngine.UIElements.Button;

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
        private MainController mainController;
        private bool isActivePageCreateGameFile;
        
        private const string NULL_TEXT_INPUT = "";

        public GameObject ActivePage => activePage;
        
        private void Awake()
        {
            menuPageChangedEventChannel = Finder.MenuPageChangedEventChannel;
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;

            mainController = Finder.MainController;
            dispatcher = Finder.Dispatcher;
            saveSystem = Finder.SaveSystem;
            activePage = firstPage;
            isActivePageCreateGameFile = false;
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
            if (fileName.text != NULL_TEXT_INPUT)
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
        public void OpenPopup(GameObject saveToDelete)
        {
            this.saveToDelete = saveToDelete;
            SetPage(popupWindow);
        }
        
        [UsedImplicitly]
        public void ExitGame()
        {
            mainController.ExitGame();
        }
    }
}