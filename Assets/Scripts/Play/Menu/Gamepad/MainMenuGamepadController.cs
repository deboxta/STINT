using Harmony;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;
//Author : Yannick Cote
namespace Game
{
    public class MainMenuGamepadController : MenuGamepadController
    {
        private MenuPageChangedEventChannel menuPageChangedEventChannel;

        private Button firstButton;
        private Button returnButton;
        
        private void Awake()
        {
            menuPageChangedEventChannel = Finder.MenuPageChangedEventChannel;
            firstButton = GetComponentInChildren<Image>().GetComponentInChildren<Button>();

            returnButton = null;
        }

        protected override void Start()
        {
            isReturnButtonNotNull = returnButton != null;
            isFirstButtonNotNull = firstButton != null;
            SelectFirstButton(firstButton);
            base.Start();
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
            /*if (menuController.ActivePage.name != "Menu")
                returnButton = FindReturnButton();*/

            SelectFirstButton(firstButton);
        }
        
        private Button FindReturnButton()
        {
            //For beta
            //return GameObject.FindGameObjectWithTag(R.S.Tag.ReturnButton).GetComponentInChildren<Button>();
            return null;
        }
    }
}