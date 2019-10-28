using System;
using System.Diagnostics.Tracing;
using Harmony;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XInputDotNetPure;

namespace Game
{
    public class PauseMenuController : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button resumeButton = null;
        [SerializeField] private Button exitButton = null;

        private LevelController levelController;
        private GamePadState gamePadState;
        private Canvas canvas;

        private static Button currentSelectedButton =>
            EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();

        private bool CanvasEnabled => canvas.enabled;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            levelController = Finder.LevelController;
        }

        private void Start()
        {
            canvas.enabled = false;
        }

        private void Update()
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);
            
            if (!CanvasEnabled)
            {
                if (gamePadState.Buttons.Start == ButtonState.Pressed) Pause();
            }
            else
            {
                if (gamePadState.ThumbSticks.Left.Y < 0) 
                    UIExtenssions.SelectedButton?.SelectDown();
                else if (gamePadState.ThumbSticks.Right.Y > 0)
                    UIExtenssions.SelectedButton?.SelectUp();
                else if (gamePadState.Buttons.A == ButtonState.Released)
                    if (gamePadState.Buttons.A == ButtonState.Pressed)
                        UIExtenssions.SelectedButton?.Click();

            }
        }
        
        [UsedImplicitly]
        public void Pause()
        {
            Time.timeScale = 0;
            canvas.enabled = true;
            resumeButton.Select();
        }
        
        [UsedImplicitly]
        public void Resume()
        {
            Time.timeScale = 1;
            canvas.enabled = false;
        }
        
        [UsedImplicitly]
        public void Exit()
        {
            Time.timeScale = 1;
            canvas.enabled = false;
            levelController.ReturnToMainMenu();
        }
    }
}