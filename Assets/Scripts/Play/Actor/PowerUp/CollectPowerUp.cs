using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Jeammy Côté
    public class CollectPowerUp : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CompareTag(R.S.Tag.Collectable))
            {
                Finder.Player.CollectPowerUp();
                var powerUp = GetComponentInParent<PowerUp>();
                if (powerUp != null)
                {
                    powerUp.Collect();
                }
            }
            else if (CompareTag(R.S.Tag.Boots))
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