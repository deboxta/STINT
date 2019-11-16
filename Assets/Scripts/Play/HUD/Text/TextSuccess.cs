using System;
using System.Collections;
using Harmony;
using TMPro;
using UnityEngine;

namespace Game
{
    public class TextSuccess : MonoBehaviour
    {
        [SerializeField] private float displayTime = 5f;
        
        private SuccessUnlockedEventChannel successUnlockedEventChannel;
        private TextMeshProUGUI textMeshProUGUI;

        private void Awake()
        {
            successUnlockedEventChannel = Finder.SuccessUnlockedEventChannel;
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            successUnlockedEventChannel.OnSuccessUnlocked += OnSuccessUnlocked;
        }

        private void OnDisable()
        {
            successUnlockedEventChannel.OnSuccessUnlocked -= OnSuccessUnlocked;
        }

        private void OnSuccessUnlocked(string successName)
        {
            textMeshProUGUI.text = successName;
            StartCoroutine(DisplayText());
        }

        private IEnumerator DisplayText()
        {
            yield return new WaitForSeconds(displayTime);
            textMeshProUGUI.text = "";
        }
    }
}