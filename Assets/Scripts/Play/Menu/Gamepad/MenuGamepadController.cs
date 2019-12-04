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
            if (isFirstButtonNotNull && firstButton != null)
                firstButton.Select();
        }
        
        protected virtual void Update()
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);
        }
    }
}