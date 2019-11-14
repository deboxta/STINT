using System.Collections;
using Harmony;
using TMPro;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
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

            //BC : Attribut qui devrait être une variable.
            //     C'est quelque chose que vous devriez vraiment faire attention.
            texts = textsToDisplay;
            
            //BC : Même chose pour ça je pense.
            displayedText = "";
            textIndex = 0;
            characterIndex = 0;
                
            StartCoroutine(DisplayOneCharacter());
        }

        //BC : Nommage.
        private IEnumerator DisplayOneCharacter()
        {
            if (textIndex < texts.Length)
            {
                if (characterIndex < texts[textIndex].Length + 1)
                {
                    //BR : Tu peux boucler sur les caractères d'une chaine de caractère.
                    /*
                    foreach (var character in texts[textIndex])
                    {
                        //Ajouter le caractère
                        yeild return new WaitForSeconds(delayBetweenCharacters);
                    }
                    */
                    //     Tu as, pour ainsi dire, la solution dans ta face à présent, je
                    //     te laisse terminer.
                    
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
            //BC : OH NON. Une coroutine récursive!!!!
            //     Si votre chaine de caractère est trop long, c'est le "StackOverflow"
            //     qui vous attends.
            //
            //     Utilisez une boucle SVP!
            yield return DisplayOneCharacter();
        }
    }
}