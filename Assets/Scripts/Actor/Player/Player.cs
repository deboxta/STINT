using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using XInputDotNetPure;
namespace Game
{
    public class Player : MonoBehaviour
    {
        private PlayerMover mover;
        static private GamePadState gamePad = GamePad.GetState(PlayerIndex.One);


        private void Awake()
        {
            mover = GetComponent<PlayerMover>();
        }

        private void Update()
        {
            var direction = Vector2.zero;
            if (Input.GetKey(KeyCode.D) ||
                gamePad.ThumbSticks.Left.X > 0)
                direction += Vector2.right;
            if (Input.GetKey(KeyCode.A) ||
                gamePad.ThumbSticks.Left.X < 0)
                direction += Vector2.left;
            mover.Move(direction);
            
            if (Input.GetKeyDown(KeyCode.Space) || 
                 gamePad.Buttons.A == ButtonState.Pressed)
            {
                mover.Jump();
            }
        }
    }
}