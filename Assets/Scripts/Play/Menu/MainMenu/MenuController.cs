using Game;
using Harmony;
using JetBrains.Annotations;
using UnityEngine;

//Author : Yannick Cote
namespace Play.Menu.MainMenu
{
    [Findable(R.S.Tag.MenuController)]
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject[] menuPages = null;
        [SerializeField] private GameObject popupWindow = null;
        
        private LevelCompletedEventChannel levelCompletedEventChannel;
        private MenuPageChangedEventChannel menuPageChangedEventChannel;
        
        private GameObject activePage;

        public GameObject ActivePage => activePage;

        private void Awake()
        {
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
            menuPageChangedEventChannel = Finder.MenuPageChangedEventChannel;
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
        public void StartGame()
        {
            levelCompletedEventChannel.NotifyLevelCompleted();
        }

        [UsedImplicitly]
        public void SetPage(GameObject toEnable)
        {
            activePage.SetActive(false);
            toEnable.SetActive(true);
            menuPageChangedEventChannel.NotifyPageChanged();
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