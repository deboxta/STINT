using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    public class PlayerInput : MonoBehaviour
    {
        private GamePadState gamePadState;
        private PlayerMover playerMover;
        private Player player;
        private bool viewingRight;

        private void Awake()
        {
            playerMover = GetComponent<PlayerMover>();
            player = GetComponent<Player>();
        }

        private void Update()
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);

            var direction = Vector2.zero;

            //Right
            if (Input.GetKey(KeyCode.D) ||
                gamePadState.ThumbSticks.Left.X > 0)
            {
                direction += Vector2.right;
                player.IsLookingRight = true;
            }

            //Left
            if (Input.GetKey(KeyCode.A) ||
                gamePadState.ThumbSticks.Left.X < 0)
            {
                {
                    direction += Vector2.left;
                    player.IsLookingRight = false;
                }
            }

            playerMover.Move(direction);

            //Jump
            if (Input.GetKeyDown(KeyCode.Space) || gamePadState.Buttons.A == ButtonState.Pressed)
            {
                playerMover.Jump();
            }
            
            //Fall
            if (gamePadState.Buttons.A == ButtonState.Released)
            {
                playerMover.Fall();
            }

            //Grab
            if (Input.GetKeyDown(KeyCode.C) ||
                 GamePad.GetState(PlayerIndex.One).Triggers.Right > 0)
                player.GrabBox();
            
            //Throw
            if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0 == false  && player.Hands.IsHoldingBox)
                player.ThrowBox();
        }
    }
}