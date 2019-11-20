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

        private static readonly string SPEED = "Speed";
        private static readonly string IS_JUMPING = "IsJumping";
        private static readonly string IS_WALL_SLIDING = "IsWallSliding";

        private SpriteRenderer playerSpriteRenderer;
        private bool coroutineIsRunning = false;
        
        private void Awake()
        {
            playerSpriteRenderer = Finder.Player.GetComponentInChildren<SpriteRenderer>();
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
            animator.SetFloat(SPEED,Mathf.Abs(speed));
        }

        public void WallJumpWarningAnimation()
        {
            if (!coroutineIsRunning)
            {
                StartCoroutine(WallJumpWarningTimeLaps());
            }
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