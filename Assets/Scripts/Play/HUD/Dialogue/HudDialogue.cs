using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HudDialogue : MonoBehaviour
    {
        [SerializeField] private string[] fullText;
        [SerializeField] private float delayBetweenCharacters = 0.1f;
        [SerializeField] private float delayBetweenTexts = 3f;
        private TextMeshProUGUI text;
        private int characterIndex;
        private int textIndex;
        private string displayedText;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            displayedText = "";
            characterIndex = 0;
            textIndex = 0;

            StartCoroutine(DisplayOneCharacter());
        }

        private IEnumerator DisplayOneCharacter()
        {
            displayedText = fullText[textIndex].Substring(0, characterIndex);
            text.text = displayedText;
            characterIndex++;
            
            yield return new WaitForSeconds(delayBetweenCharacters);

            if (characterIndex > fullText[textIndex].Length)
            {
                if (textIndex >= fullText.Length - 1)
                {
                    text.enabled = false;
                    yield break;
                }
                textIndex++;
                characterIndex = 0;
                
                yield return new WaitForSeconds(delayBetweenTexts);
            }
            yield return DisplayOneCharacter();
        }
    }
}