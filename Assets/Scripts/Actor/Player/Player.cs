using Harmony;
using UnityEngine;
using XInputDotNetPure;
namespace Game
{
    public class Player : MonoBehaviour
    {
        private PlayerHitEventChannel playerHitEventChannel;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private PlayerMover mover;

        private void Awake()
        {
            playerHitEventChannel = Finder.PlayerHitEventChannel;
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
            mover = GetComponent<PlayerMover>();
        }
        
        private void OnEnable()
        {
            playerHitEventChannel.OnPlayerHit += Hit;
        }

        private void OnDisable()
        {
            playerHitEventChannel.OnPlayerHit -= Hit;
        }

        private void Update()
        {
            var direction = Vector2.zero;
            if (Input.GetKey(KeyCode.D) ||
                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
                direction += Vector2.right;
            if (Input.GetKey(KeyCode.A) ||
               GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
                direction += Vector2.left;
            mover.Move(direction);
            
            if (Input.GetKeyDown(KeyCode.Space) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
            {
                mover.Jump();
            }
        }
        
        private void Hit()
        {
            Die();
        }

        private void Die()
        {
            playerDeathEventChannel.NotifyPlayerDeath();
        }
    }
}