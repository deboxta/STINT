﻿using System;
using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    //Author : Sébastien Arsenault
    public class TextDefiler : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private float end;

        private RectTransform rectTransform;
        private Text text;
        private SceneController sceneController;
        private GameWonEventChannel gameWonEventChannel;
        
        private void Awake()
        {
            rectTransform = GetComponent<Canvas>().GetComponent<RectTransform>();
            text = GetComponentInChildren<Text>();

            sceneController = Finder.SceneController;
            gameWonEventChannel = Finder.GameWonEventChannel;
        }

        private IEnumerator Start()
        {
            //This is use because it the end of the game the success detector need to know that the player won the game
            gameWonEventChannel.NotifyGameWon();
            
            //Certain necessary components might not be initialize at this point so we need to wait for one update
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
            
            sceneController.ReturnToMainMenu();
        }
    }
}