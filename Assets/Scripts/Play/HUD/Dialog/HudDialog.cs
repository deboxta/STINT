using System.Collections;
using Harmony;
using TMPro;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    [Findable(R.S.Tag.HudController)]
    public class HudDialog : MonoBehaviour
    {
        [SerializeField] private float delayBetweenCharacters = 0.1f;
        [SerializeField] private float delayBetweenTexts = 3f;

        private TextMeshProUGUI textMesh;
        private int textIndex;
        private string displayedText;

        private void Awake()
        {
            textMesh = GetComponentInChildren<TextMeshProUGUI>();
            displayedText = "";
            textIndex = 0;
        }

        public void StartDisplaying(string[] textsToDisplay)
        {
            //All coroutine needs to stop or it create bugs
            StopAllCoroutines();

            displayedText = "";
            textIndex = 0;

            StartCoroutine(DisplayText(textsToDisplay));
        }

        private IEnumerator DisplayText(string[] textsToDisplay)
        {
            foreach (var text in textsToDisplay)
            {
                foreach (var character in textsToDisplay[textIndex])
                {
                    displayedText += character;
                    textMesh.text = displayedText;

                    yield return new WaitForSeconds(delayBetweenCharacters);
                }

                displayedText = "";
                textIndex++;

                yield return new WaitForSeconds(delayBetweenTexts);
            }
            
            textMesh.text = "";
        }
    }
}