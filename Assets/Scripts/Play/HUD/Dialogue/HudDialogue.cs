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

        public string[] Texts
        {
            get => texts;
            set => texts = value;
        }

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
            displayedText = Texts[textIndex].Substring(0, characterIndex);
            textMesh.text = displayedText;
            characterIndex++;
            
            yield return new WaitForSeconds(delayBetweenCharacters);

            if (characterIndex >= Texts[textIndex].Length)
            {
                if (textIndex >= Texts.Length - 1)
                {
                    textMesh.text = "";
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