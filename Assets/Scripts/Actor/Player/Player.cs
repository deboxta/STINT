using UnityEngine;
using XInputDotNetPure;
namespace Game
{
    public class Player : MonoBehaviour
    {
        private PlayerMover mover;

        private void Awake()
        {
            mover = GetComponent<PlayerMover>();
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
    }
}