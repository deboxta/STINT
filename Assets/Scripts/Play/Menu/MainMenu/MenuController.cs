using System;
using Game;
using Harmony;
using UnityEngine;
using UnityEngine.UI;


namespace Play.Menu.MainMenu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Light light = null;
        private LevelCompletedEventChannel levelCompletedEventChannel;

        private void Awake()
        {
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
        }

        public void StartGame()
        {
            levelCompletedEventChannel.NotifyLevelCompleted();
        }

        public void SetLuminosity(Scrollbar scrollbar)
        {
            light.intensity = scrollbar.value;
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