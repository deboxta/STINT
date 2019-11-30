﻿using Harmony;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;
//Author : Yannick Cote
//TODO (GOLD) : SEPARATE THIS CLASS IN TWO (ONE FOR THE PAUSE MENU AND ONE FOR THE MENU)
namespace Game
{
    public class MenuGamepadController : MonoBehaviour
    {
        [Header("Type menu")] 
        [SerializeField] private bool isMainMenu = true;
        
        [Header("Pause menu")]
        [SerializeField] private GameObject body = null;

        private SceneController sceneController;
        private MenuController menuController;
        private GamePadState gamePadState;
        private MenuPageChangedEventChannel menuPageChangedEventChannel;

        private Button firstButton;
        private Button returnButton;
        private bool isReturnButtonPressed;
        private bool isFirstButtonPressed;
         
        private Canvas canvas; 
        private bool isfirstButtonNotNull;
        private bool isbodyNotNull;
        private bool isreturnButtonNotNull;
        private void Awake()
        {
            if (!isMainMenu)
            {
                canvas = GetComponent<Canvas>();
                firstButton = GetComponentInChildren<Button>();
                body.SetActive(false);
            }
            else
            {
                menuPageChangedEventChannel = Finder.MenuPageChangedEventChannel;
                canvas = GetComponentInParent<Canvas>();
                firstButton = GetComponentInChildren<Image>().GetComponentInChildren<Button>();
            }

            returnButton = null;
            sceneController = Finder.SceneController;
            menuController = Finder.MenuController;
        }

        private void Start()
        {
            isreturnButtonNotNull = returnButton != null;
            isbodyNotNull = body != null;
            isfirstButtonNotNull = firstButton != null;
            isFirstButtonPressed = false;
            isReturnButtonPressed = false;
            SelectFirstButton();
        }

        private void OnEnable()
        {
            if (menuPageChangedEventChannel != null)
                menuPageChangedEventChannel.OnPageChanged += PageChanged;
        }

        private void OnDisable()
        {
            if (menuPageChangedEventChannel != null)
                menuPageChangedEventChannel.OnPageChanged -= PageChanged;
        }

        private void PageChanged()
        {
            firstButton = GetComponentInChildren<Button>();
            if (menuController.ActivePage.name != "Menu")
                returnButton = FindReturnButton();

            SelectFirstButton();
        }

        private void SelectFirstButton()
        {
            if (isfirstButtonNotNull)
                firstButton.Select();
        }

        private Button FindReturnButton()
        {
            //For beta
            //return GameObject.FindGameObjectWithTag(R.S.Tag.ReturnButton).GetComponentInChildren<Button>();
            return null;
        }

        private void Update()
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);
            
            if (isbodyNotNull && !body.activeSelf)
            {
                if (gamePadState.Buttons.Start == ButtonState.Pressed && sceneController.CurrentLevel != 0) Pause();
            }
            else
            {
                if (gamePadState.ThumbSticks.Left.Y < 0) 
                    UIExtensions.SelectedButton?.SelectDown();
                else if (gamePadState.ThumbSticks.Right.Y > 0)
                    UIExtensions.SelectedButton?.SelectUp();
                else if (gamePadState.Buttons.B == ButtonState.Pressed)
                    isReturnButtonPressed = true;

                else if (gamePadState.Buttons.B == ButtonState.Released)
                    isReturnButtonPressed = false;

                else if (gamePadState.Buttons.A == ButtonState.Released)
                    isReturnButtonPressed = false;

                else if (gamePadState.Buttons.A == ButtonState.Pressed)
                    isFirstButtonPressed = true;

                if (isFirstButtonPressed)
                    UIExtensions.SelectedButton?.Click();

                if (isReturnButtonPressed)
                {
                    if (isreturnButtonNotNull)
                    {
                        returnButton.Select();
                        UIExtensions.SelectedButton?.Click();
                    }
                }
            }
        }
        
        [UsedImplicitly]
        public void Pause()
        {
            Time.timeScale = 0;
            body.SetActive(true);
            SelectFirstButton();
        }
        
        [UsedImplicitly]
        public void Resume()
        {
            Time.timeScale = 1;
            body.SetActive(false);
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