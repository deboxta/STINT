using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    public class PlayerJumpGravity : MonoBehaviour
    {
        [SerializeField] private int fallGravity = 1;
        [SerializeField] private int tinyJumpGravity = 5;

        private new Rigidbody2D rigidbody2D;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void PlayerJumpGravityUpdate(GamePadState gamePadState)
        {
            if (rigidbody2D.velocity.y < 0)
            {
                rigidbody2D.velocity += Time.deltaTime * Physics2D.gravity.y * (fallGravity - 1) * Vector2.up;
            }
            else if (rigidbody2D.velocity.y > 0 && gamePadState.Buttons.A == ButtonState.Released)
            {
                rigidbody2D.velocity += Time.deltaTime * Physics2D.gravity.y * (tinyJumpGravity - 1) * Vector2.up;
            }
        }
    }
}