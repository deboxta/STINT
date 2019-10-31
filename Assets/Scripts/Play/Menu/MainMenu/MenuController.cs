using System;
using Game;
using Harmony;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;


namespace Play.Menu.MainMenu
{
    [Findable(R.S.Tag.MenuController)]
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject[] menuPages = null;
        [SerializeField] private GameObject popupWindow = null;
        
        private LevelCompletedEventChannel levelCompletedEventChannel;
        private GameObject activePage;
        private MenuPageChangedEventChannel menuPageChangedEventChannel;

        public GameObject ActivePage => activePage;

        private void Awake()
        {
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
            menuPageChangedEventChannel = Finder.MenuPageChangedEventChannel;
        }
        
        private GameObject GetActiveImage()
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
            activePage = GetActiveImage();
        }

        public void StartGame()
        {
            levelCompletedEventChannel.NotifyLevelCompleted();
        }

        public void SetPage(GameObject toEnable)
        {
            activePage.SetActive(false);
            toEnable.SetActive(true);
            menuPageChangedEventChannel.NotifyPageChanged();
        }

        public void ClosePopup()
        {
            popupWindow.SetActive(false);
        }
        
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