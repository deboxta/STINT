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
        
        private void Awake()
        {
            menuPageChangedEventChannel = Finder.MenuPageChangedEventChannel;
            firstButton = GetComponentInChildren<Image>().GetComponentInChildren<Button>();
        }

        protected override void Start()
        {
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

            SelectFirstButton(firstButton);
        }
    }
}