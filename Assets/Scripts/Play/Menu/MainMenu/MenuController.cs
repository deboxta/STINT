using Harmony;
using JetBrains.Annotations;
using UnityEngine;

//Author : Yannick Cote
namespace Game
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
            //BR : Devrait être remplacé par un boucle "for each".
            //BC : Patch (en lien avec mon commentaire dans "SetPage").
            for (int i = 0; i < menuPages.Length; i++)
            {
                //BR : "== true" inutile.
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
            //BC : Woah minute. Pourquoi ne conserve tu pas le paramêtre reçu ici
            //     en attribut ? Cela éviterait de faire une recherche à chaque "Frame" dans
            //     GetActivePage.
            activePage.SetActive(false);
            toEnable.SetActive(true);
            menuPageChangedEventChannel.NotifyPageChanged();
        }
        
        [UsedImplicitly]
        public void ClosePopup()
        {
            popupWindow.SetActive(false);
        }
        
        //BR : Devrait demander au "MainController" de faire cela au lieu de le faire ici.
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