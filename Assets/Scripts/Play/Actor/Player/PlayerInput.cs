using System;
using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    public class PlayerInput : MonoBehaviour
    {
        private const string FLOOR_LAYER_ID = "Floor";
        private GamePadState gamePadState;
        private PlayerMover playerMover;
        private PlayerJumpGravity playerJumpGravity;
        
        private bool isGrounded;

        private void Awake()
        {
            playerMover = GetComponent<PlayerMover>();
            playerJumpGravity = GetComponent<PlayerJumpGravity>();
            
            isGrounded = true;
        }

        private void Update()
        { 
            gamePadState = GamePad.GetState(PlayerIndex.One);
            
            var direction = Vector2.zero;
            
            //Right
            if (Input.GetKey(KeyCode.D) ||
                gamePadState.ThumbSticks.Left.X > 0)
                direction += Vector2.right;
            
            //Left
            if (Input.GetKey(KeyCode.A) ||
                gamePadState.ThumbSticks.Left.X < 0)
                direction += Vector2.left;
            
            playerMover.Move(direction);
            
            //Jump
            if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || gamePadState.Buttons.A == ButtonState.Pressed))
            {
                isGrounded = false;
                playerMover.Jump();
            }

            //Affected gravity in the air if jump button is pressed or not
            playerJumpGravity.PlayerJumpGravityUpdate(gamePadState);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer(FLOOR_LAYER_ID))
                isGrounded = true;
        }
    }
}