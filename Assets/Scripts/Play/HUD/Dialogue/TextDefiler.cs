using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TextDefiler : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;

        private RectTransform rectTransform;
        private Text text;
        [SerializeField] private float end;
        
        private void Awake()
        {
            rectTransform = GetComponent<Canvas>().GetComponent<RectTransform>();
            text = GetComponentInChildren<Text>();
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
        }

        private void FixedUpdate()
        {
            var newPosition = text.transform.localPosition;

            if (newPosition.y < end)
            {
                newPosition.Set(newPosition.x, newPosition.y + speed, newPosition.z);
            
                text.transform.localPosition = newPosition;
            }
        }
    }
}