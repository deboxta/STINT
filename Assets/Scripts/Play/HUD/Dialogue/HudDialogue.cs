using System.Collections;
using Harmony;
using TMPro;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.HudController)]
    public class HudDialogue : MonoBehaviour
    {
        [SerializeField] private float delayBetweenCharacters = 0.1f;
        [SerializeField] private float delayBetweenTexts = 3f;
        
        private string[] texts;
        private TextMeshProUGUI textMesh;
        private int characterIndex;
        private int textIndex;
        private string displayedText;

        //private string[] Texts => texts;

        private void Awake()
        {
            textMesh = GetComponentInChildren<TextMeshProUGUI>();
            displayedText = "";
            characterIndex = 0;
            textIndex = 0;
        }

        public void StartDisplaying(string[] textsToDisplay)
        {
            //All coroutine needs to stop or it create
            StopAllCoroutines();

            texts = textsToDisplay;
            
            displayedText = "";
            textIndex = 0;
            characterIndex = 0;
                
            StartCoroutine(DisplayOneCharacter());
        }

        private IEnumerator DisplayOneCharacter()
        {
            if (textIndex < texts.Length)
            {
                if (characterIndex < texts[textIndex].Length + 1)
                {
                    displayedText = texts[textIndex].Substring(0, characterIndex);
                    textMesh.text = displayedText;
                    characterIndex++;
                    
                    yield return new WaitForSeconds(delayBetweenCharacters);
                }
                else
                {
                    textIndex++;
                    characterIndex = 0;
                    
                    yield return new WaitForSeconds(delayBetweenTexts);
                }
            }
            else
            {
                textMesh.text = "";
                yield break;
            }
            yield return DisplayOneCharacter();
        }
    }
}