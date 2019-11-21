using System;
using Harmony;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public class DeadlyGridTrap : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other1)
        {
            if (other1.transform.CompareTag(R.S.Tag.Player))
            {
                Finder.Player.Die();
            }
        }
    }
}