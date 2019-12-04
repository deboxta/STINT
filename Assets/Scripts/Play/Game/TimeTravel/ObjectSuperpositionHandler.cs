using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Jeammy Côté
    public class ObjectSuperpositionHandler : MonoBehaviour
    {
        private Player player;
        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //TODO: At this time the way we compare the bounds of the player is working, but it would be better to check two specific points later.
            
            if (other.CompareTag(R.S.Tag.DeathZone) &&
                other.bounds.Contains(player.transform.position))
            {
                player.Die();
            }
            
        }
    }
}