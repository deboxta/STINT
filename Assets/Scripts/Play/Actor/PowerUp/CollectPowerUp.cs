using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Jeammy Côté
    //TODO : Change with inheritance
    public class CollectPowerUp : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var otherParent = other.Parent();
            if (CompareTag(R.S.Tag.Collectable) && other.Parent().CompareTag(R.S.Tag.Player))
            {
                Finder.Player.CollectPowerUp();
                var powerUp = GetComponentInParent<PowerUp>();
                if (powerUp != null)
                {
                    powerUp.Collect();
                }
            }
            else if (otherParent != null) 
            {
                if (CompareTag(R.S.Tag.Boots) && otherParent.CompareTag(R.S.Tag.Player))
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