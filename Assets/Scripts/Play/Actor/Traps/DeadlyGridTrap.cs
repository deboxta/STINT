using System;
using Harmony;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public class DeadlyGridTrap : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.CompareTag(R.S.Tag.Player))
            {
                Finder.Player.Die();
            }
        }
    }
}