using System;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    public class PlayerInput : MonoBehaviour
    {
        private const string FLOOR_LAYER_ID = "Floor";
        private GamePadState gamePadState;
        private PlayerMover playerMover;
        private Player player;
        private bool viewingRight;
        
        private bool isGrounded;

        private void Awake()
        {
            playerMover = GetComponent<PlayerMover>();
            player = GetComponent<Player>();

            isGrounded = true;
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
            if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || gamePadState.Buttons.A == ButtonState.Pressed))
            {
                isGrounded = false;
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
            if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0 == false  && player.IsHoldingBox)
                player.ThrowBox();
        }

        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer(FLOOR_LAYER_ID))
                isGrounded = true;
        }
    }
}