using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private KeyCode CHANGE_TIMELINE_KEYBOARD_KEY = KeyCode.LeftShift;

        private GamePadState gamePadState;
        private PlayerMover playerMover;
        private Player player;
        private bool viewingRight;
        private bool crouching;
        private bool timeChangeIsClicked;

        private void Awake()
        {
            playerMover = GetComponent<PlayerMover>();
            player = GetComponent<Player>();

            viewingRight = false;
            crouching = false;
            timeChangeIsClicked = false;
        }

        private void Update()
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);

            //Crouch
            if (gamePadState.ThumbSticks.Left.Y < 0 && gamePadState.ThumbSticks.Left.X == 0)
                crouching = true;
            else
                crouching = false;

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
                direction += Vector2.left;
                player.IsLookingRight = false;
            }

            playerMover.Move(direction);

            //Jump
            if (Input.GetKeyDown(KeyCode.Space) || gamePadState.Buttons.A == ButtonState.Pressed)
                playerMover.Jump();

            //Switch timeline
            if (gamePadState.Buttons.X == ButtonState.Pressed ||
                gamePadState.Buttons.Y == ButtonState.Pressed)
                timeChangeIsClicked = true;
            else if (Input.GetKeyDown(CHANGE_TIMELINE_KEYBOARD_KEY) ||
                     gamePadState.Buttons.X == ButtonState.Released && timeChangeIsClicked ||
                     gamePadState.Buttons.Y == ButtonState.Released && timeChangeIsClicked)
            {
                Finder.TimelineController.SwitchTimeline();
                timeChangeIsClicked = false;
            }

            //Fall
            if (gamePadState.Buttons.A == ButtonState.Released)
                playerMover.Fall();

            //Grab
            if (Input.GetKeyDown(KeyCode.C) ||
                GamePad.GetState(PlayerIndex.One).Triggers.Right > 0)
                player.GrabBox();

            //Throw
            if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0 == false && player.Hands.IsHoldingBox)
                player.ThrowBox(crouching);
        }
    }
}