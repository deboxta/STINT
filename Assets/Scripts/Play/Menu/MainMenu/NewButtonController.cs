using System;
using Harmony;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

namespace Game
{
    public class NewButtonController : MonoBehaviour
    {
        [SerializeField] private GameObject newButton;
        private SaveSystem saveSystem;
        private void Awake()
        {
            saveSystem = Finder.SaveSystem;
            SetNewButton();
        }

        [UsedImplicitly]
        public void SetNewButton()
        {
            if (saveSystem.NbOfSaves >= 3)
            {
                newButton.SetActive(false);
            }
            else
            {
                newButton.SetActive(true);
            }
        }
    }
}