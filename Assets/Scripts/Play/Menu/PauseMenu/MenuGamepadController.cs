using System;
using System.Collections;
using System.Diagnostics.Tracing;
using Harmony;
using JetBrains.Annotations;
using Play.Menu.MainMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XInputDotNetPure;

namespace Game
{
    public class MenuGamepadController : MonoBehaviour
    {
        [Header("Type menu")]
        [SerializeField] private bool isMainMenu = true;

        private LevelController levelController;
        private GamePadState gamePadState;
        private MenuPageChangedEventChannel menuPageChangedEventChannel;

        private Button firstButton;
        private Canvas canvas; 
        private bool isfirstButtonNotNull;

        private bool CanvasEnabled => canvas.enabled;

        private void Awake()
        {
            if (!isMainMenu)
            {
                canvas = GetComponent<Canvas>();
                firstButton = GetComponentInChildren<Button>();
                canvas.enabled = false;
            }
            else
            {
                menuPageChangedEventChannel = Finder.MenuPageChangedEventChannel;
                canvas = GetComponentInParent<Canvas>();
                firstButton = GetComponentInChildren<Image>().GetComponentInChildren<Button>();
            }
            levelController = Finder.LevelController;
        }

        private void Start()
        {
            isfirstButtonNotNull = firstButton != null;
            SelectFirstButton();
        }

        private void OnEnable()
        {
            if (menuPageChangedEventChannel != null)
            {
                menuPageChangedEventChannel.OnPageChanged += PageChanged;
            }
        }

        private void OnDisable()
        {
            if (menuPageChangedEventChannel != null)
            {
                menuPageChangedEventChannel.OnPageChanged -= PageChanged;
            }
        }

        private void PageChanged()
        {
            firstButton = GetComponentInChildren<Button>();
            SelectFirstButton();
        }

        private void SelectFirstButton()
        {
            if (isfirstButtonNotNull)
                firstButton.Select();
        }

        private void Update()
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);
            
            if (!CanvasEnabled)
            {
                if (gamePadState.Buttons.Start == ButtonState.Pressed && levelController.CurrentLevel != 0) Pause();
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
            SelectFirstButton();
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