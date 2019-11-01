using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TextDefiler : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private float end;

        private RectTransform rectTransform;
        private Text text;
        private LevelController levelController;
        
        private void Awake()
        {
            rectTransform = GetComponent<Canvas>().GetComponent<RectTransform>();
            text = GetComponentInChildren<Text>();

            levelController = Finder.LevelController;
        }

        private IEnumerator Start()
        {
            yield return null;
            
            var textRectTransform = text.rectTransform;
            var screenRectTransform = rectTransform;
            
            var textHeight = textRectTransform.rect.height / 2;
            var screenHeight = screenRectTransform.rect.height / 2;

            end = textHeight + screenHeight;

            textRectTransform.localPosition = new Vector3(0, -end, 0);

            var newPosition = text.transform.localPosition;
            
            while (newPosition.y < end)
            {
                newPosition.Set(newPosition.x, newPosition.y + speed, newPosition.z);
            
                text.transform.localPosition = newPosition;

                yield return null;
            }
            //TODO replace this with the code Yannick made to return to the main menu
            levelController.ReturnToMainMenu();
        }
    }
}