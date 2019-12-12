using Harmony;
using UnityEngine;

namespace Game
{
    public class CollectibleBoots : Collectible
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var otherParent = other.Parent();
            if (otherParent != null) 
            {
                if (other.transform.parent != null && otherParent.CompareTag(R.S.Tag.Player))
                {
                    Finder.Player.CollectBoots();
                    var wallJumpBoots = GetComponentInParent<WallJumpBoots>();
                    if (wallJumpBoots != null)
                    {
                        wallJumpBoots.Collect();
                    }
                }
            }
        }
    }
}