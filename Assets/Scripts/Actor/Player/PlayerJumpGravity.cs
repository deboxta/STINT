using System;
using UnityEngine;
using XInputDotNetPure;

namespace Game
{
    public class PlayerJumpGravity : MonoBehaviour
    {
        private int fallGravity = 3;
        private int tinyJumpGravity = 2;

        private Rigidbody2D rigidbody2D;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (rigidbody2D.velocity.y < 0)
            {
                rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallGravity - 1) * Time.deltaTime;
            }
            else if (rigidbody2D.velocity.y > 0 && GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Released)
            {
                rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (tinyJumpGravity - 1) * Time.deltaTime;
            }
        }
    }
}