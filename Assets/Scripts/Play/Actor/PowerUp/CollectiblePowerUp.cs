using Harmony;
using UnityEngine;

namespace Game
{
    public class CollectiblePowerUp : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CompareTag(R.S.Tag.Collectable) && other.Parent().CompareTag(R.S.Tag.Player))
            {
                Finder.Player.CollectPowerUp();
                var powerUp = GetComponentInParent<PowerUp>();
                if (powerUp != null)
                {
                    powerUp.Collect();
                }
            }
        }
    }
}