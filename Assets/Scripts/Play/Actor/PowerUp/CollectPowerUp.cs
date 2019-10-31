using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class CollectPowerUp : MonoBehaviour
    {
        private void Awake()
        {
            tag = R.S.Tag.Collectable;
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CompareTag(tag))
            {
                other.GetComponentInParent<Player>().CollectPowerUp();
                GetComponentInParent<PowerUp>().Collect();
            }
        }
    }
}