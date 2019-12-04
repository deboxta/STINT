using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

namespace Game
{
    public class MenuGamepadController : MonoBehaviour
    {
        protected GamePadState gamePadState;
        
        protected bool isFirstButtonNotNull;
        protected bool isBodyNotNull;
        protected bool isFirstButtonPressed;

                
        protected virtual void Start()
        {
            isFirstButtonPressed = false;
        }
        
        private protected void SelectFirstButton(Button firstButton)
        {
            if (isFirstButtonNotNull)
                firstButton.Select();
        }
        
        protected virtual void Update()
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);
            
            if (gamePadState.ThumbSticks.Left.Y < 0) 
                UIExtensions.SelectedButton?.SelectDown();
            
            else if (gamePadState.ThumbSticks.Right.Y > 0)
                UIExtensions.SelectedButton?.SelectUp();
            
            else if (gamePadState.Buttons.A == ButtonState.Released)
                isFirstButtonPressed = false;

            else if (gamePadState.Buttons.A == ButtonState.Pressed && !isFirstButtonPressed)
            {
                isFirstButtonPressed = true;
                UIExtensions.SelectedButton?.Click();
            }
            
        }
    }
}