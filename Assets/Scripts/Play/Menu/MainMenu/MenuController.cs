using UnityEngine;

namespace Play.Menu.MainMenu
{
    public class MenuController : MonoBehaviour
    {
        public void StartGame()
        {
            
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