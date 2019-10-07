using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class ObjectSuperpositionHandler : MonoBehaviour
    {
        private Player player;
        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(R.S.Tag.DeathZone) 
                && other.bounds.Contains(player.GetComponent<Rigidbody2D>().position))
            {
                //Finder.TimeController.SwitchTimeline();
                player.Die();
            }
        }
    }
}