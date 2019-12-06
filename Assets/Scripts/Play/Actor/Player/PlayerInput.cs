using System;
using System.Collections;
using Harmony;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    //Author : Anthony Bérubé
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private KeyCode changeTimelineKeyboardKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode freezeTimeKeyboardKey = KeyCode.Q;
        [SerializeField] private float inputThreshold = 0.13f;
        [SerializeField] private float timeBeforePlayerCanTimeChange = 0.5f;

        private GamePadState gamePadState;
        private PlayerMover playerMover;
        private Player player;
        private bool freezeTimeIsClicked;
        private bool jumpButtonIsPressed;
        private bool canChangeTimeline;
        private bool isChangeTimelineKeyReleased;
        private bool rtButtonUnpressed;
        private bool isPauseMenuOpen;
        private bool canPressJump;
        private float delayRegisteringJumpInput = 0.25f;

        private PauseMenuActionEventChannel pauseMenuActionEventChannel;

        private void Awake()
        {
            pauseMenuActionEventChannel = Finder.PauseMenuActionEventChannel;
            playerMover = GetComponent<PlayerMover>();
            player = GetComponent<Player>();

            freezeTimeIsClicked = false;
            canChangeTimeline = true;
            rtButtonUnpressed = true;
            canPressJump = true;
        }

        private void OnEnable()
        {
            pauseMenuActionEventChannel.OnPauseMenuAction += OnPauseMenuAction;
        }

        private void OnDisable()
        {
            pauseMenuActionEventChannel.OnPauseMenuAction -= OnPauseMenuAction;
        }

        private void OnPauseMenuAction()
        {
            switch (isPauseMenuOpen)
            {
                case true:
                    isPauseMenuOpen = false;
                    StartCoroutine(DelayBeforeGameRegisterJumpInputAfterPause());
                    break;
                case false:
                    isPauseMenuOpen = true;
                    break;
            }
        }

        private void Update()
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);
        
            if (!isPauseMenuOpen)
            {
                var direction = Vector2.zero;
                //Right
                if (Input.GetKey(KeyCode.D) ||
                    gamePadState.ThumbSticks.Left.X > inputThreshold)
                {
                    direction += Vector2.right;
                    player.IsLookingRight = true;
                    if (transform.localScale.x < 0)
                        player.FlipPlayer();
                }

                //Left
                if (Input.GetKey(KeyCode.A) ||
                    gamePadState.ThumbSticks.Left.X < -inputThreshold)
                {
                    direction += Vector2.left;
                    player.IsLookingRight = false;
                    if (transform.localScale.x > 0)
                        player.FlipPlayer();
                }

                playerMover.Move(direction);

                
                //Jump
                if ((Input.GetKeyDown(KeyCode.Space) || gamePadState.Buttons.A == ButtonState.Pressed) &&
                    !jumpButtonIsPressed && canPressJump)
                {
                    playerMover.Jump();
                    jumpButtonIsPressed = true;
                }

                if (gamePadState.Buttons.A == ButtonState.Released)
                    jumpButtonIsPressed = false;

                //Switch timeline
                if ((Input.GetKeyDown(changeTimelineKeyboardKey) ||
                     gamePadState.Buttons.X == ButtonState.Pressed ||
                     gamePadState.Buttons.Y == ButtonState.Pressed) &&
                    canChangeTimeline &&
                    isChangeTimelineKeyReleased)
                {
                    if (player.Hands.IsHoldingBox)
                        player.DropBox();
                    
                    Finder.TimelineController.SwitchTimeline();
                    StartCoroutine(TimeChangeDelay());
                    isChangeTimelineKeyReleased = false;
                }
                else if (gamePadState.Buttons.X == ButtonState.Released && gamePadState.Buttons.Y == ButtonState.Released)
                    isChangeTimelineKeyReleased = true;

    #if UNITY_EDITOR
                //Freeze time
                if (gamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
                    freezeTimeIsClicked = true;
                else if (Input.GetKeyDown(freezeTimeKeyboardKey) ||
                         gamePadState.Buttons.LeftShoulder == ButtonState.Released && freezeTimeIsClicked)
                {
                    Finder.TimeFreezeController.SwitchState();
                    freezeTimeIsClicked = false;
                }
    #endif

                //Fall
                if (gamePadState.Buttons.A == ButtonState.Released)
                    playerMover.Fall();

                //Grab
                if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0 && !player.IsHoldingBox)
                {
                    player.GrabBox();
                    rtButtonUnpressed = false;
                }
                
                //Throw
                if (gamePadState.Triggers.Right > 0 && rtButtonUnpressed && player.IsHoldingBox)
                    player.ThrowBox();
                
                //Drop
                if (gamePadState.Buttons.RightShoulder == ButtonState.Pressed && player.Hands.IsHoldingBox)
                    player.DropBox();

                //Reset Right Trigger state
                if (gamePadState.Triggers.Right > 0 == false)
                    rtButtonUnpressed = true;
            }
        }

        private IEnumerator TimeChangeDelay()
        {
            canChangeTimeline = false;
            yield return new WaitForSeconds(timeBeforePlayerCanTimeChange);
            canChangeTimeline = true;
        }

        private IEnumerator DelayBeforeGameRegisterJumpInputAfterPause()
        {
            canPressJump = false;
            yield return new WaitForSeconds(delayRegisteringJumpInput);
            canPressJump = true;
        }
    }
}