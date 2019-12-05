using System;
using Harmony;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XInputDotNetPure;

namespace Game
{
    public class PauseMenuGamepadController : MenuGamepadController
    {
        [Header("Pause menu")]
        [SerializeField] private GameObject body = null;
        
        private SceneController sceneController;

        private PauseMenuActionEventChannel pauseMenuActionEventChannel;
        private Button firstButton;

        private void Awake()
        {
            pauseMenuActionEventChannel = Finder.PauseMenuActionEventChannel;
            firstButton = body.GetComponentInChildren<Button>();
            body.SetActive(false);
            
            sceneController = Finder.SceneController;
        }

        protected override void Start()
        {
            isBodyNotNull = body != null;
            isFirstButtonNotNull = firstButton != null;
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            if (isBodyNotNull && !body.activeSelf)
            {
                if (gamePadState.Buttons.Start == ButtonState.Pressed && SceneManager.GetActiveScene().name != SceneUtility.GetScenePathByBuildIndex(5)) Pause();
            }
            
        }
        
        [UsedImplicitly]
        public void Pause()
        {
            Time.timeScale = 0;
            pauseMenuActionEventChannel.NotifyPauseMenuAction();
            EventSystem.current.SetSelectedGameObject(null);
            body.SetActive(true);
            SelectFirstButton(firstButton);
        }
        
        [UsedImplicitly]
        public void Resume()
        {
            Time.timeScale = 1;
            body.SetActive(false);
            pauseMenuActionEventChannel.NotifyPauseMenuAction();
        }
        
        [UsedImplicitly]
        public void Exit()
        {
            Time.timeScale = 1;
            body.SetActive(false);
            sceneController.ReturnToMainMenu();
        }
    }
}