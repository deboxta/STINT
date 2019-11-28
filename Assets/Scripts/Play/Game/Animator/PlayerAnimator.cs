using System;
using System.Collections;
using Harmony;
using UnityEditor.UIElements;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.Player)]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float playerWarningFlashDelay = 0.10f;

        private static readonly string IS_WALKING = "IsWalking";
        private static readonly string IS_JUMPING = "IsJumping";
        private static readonly string IS_WALL_SLIDING = "IsWallSliding";
        private static readonly string IS_GRABING_BOX = "IsGrabingBox";
        private static readonly string IS_DYING = "IsDying";
        private static readonly string IS_PRESSING = "IsPressing";

        private SpriteRenderer playerSpriteRenderer;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private LevelCompletedEventChannel levelCompletedEventChannel;
        private float movingThreshold = 0.01f;
        private bool coroutineIsRunning;
        
        private void Awake()
        {
            playerSpriteRenderer = Finder.Player.GetComponentInChildren<SpriteRenderer>();
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
        }

        private void OnEnable()
        {
            playerDeathEventChannel.OnPlayerDeath += OnPlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted += OnButtonPressing;
        }

        private void OnDisable()
        {
            playerDeathEventChannel.OnPlayerDeath -= OnPlayerDeath;
            levelCompletedEventChannel.OnLevelCompleted -= OnButtonPressing;
        }

        public void OnJumping()
        {
            animator.SetBool(IS_JUMPING, true);
        }

        public void OnLanding()
        {
            animator.SetBool(IS_JUMPING, false);
        }

        public void OnWallSliding()
        {
            animator.SetBool(IS_WALL_SLIDING, true);
        }

        public void OnStopWallSliding()
        {
            animator.SetBool(IS_WALL_SLIDING, false);
        }

        public void OnMoving(float speed)
        {
            bool isMoving = Mathf.Abs(speed) > movingThreshold;
            animator.SetBool(IS_WALKING,isMoving);
        }

        public void OnGrabBox()
        {
            animator.SetBool(IS_GRABING_BOX, true);
        }
        
        public void OnBoxThrow()
        {
            animator.SetBool(IS_GRABING_BOX, false);
        }

        public void WallJumpWarningAnimation()
        {
            if (!coroutineIsRunning)
                StartCoroutine(WallJumpWarningTimeLaps());
        }

        private void OnPlayerDeath()
        {
            animator.SetBool(IS_DYING, true);
        }

        private void OnButtonPressing()
        {
            animator.SetBool(IS_PRESSING, true);
        }

        private IEnumerator WallJumpWarningTimeLaps()
        {
            coroutineIsRunning = true;
            
            playerSpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(playerWarningFlashDelay);
            playerSpriteRenderer.color = Color.white;
            
            coroutineIsRunning = false;
        }
    }
}