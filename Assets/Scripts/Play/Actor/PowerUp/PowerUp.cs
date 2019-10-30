using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    public class PowerUp : MonoBehaviour, Collectible
    {
        [SerializeField] private int PowerUpRespawnDelay = 2;
        
        private GameObject collectable;
        
        private void Awake()
        {
            GameObject[] children = this.Children();
            foreach (var child in children)
            {
                collectable = child;
            }
        }

        private void OnEnable()
        {
            StartCoroutine(PowerUpSpawnTime());
        }

        public void Collect()
        {
            collectable.SetActive(false);
            StartCoroutine(PowerUpSpawnTime());
        }

        IEnumerator PowerUpSpawnTime()
        {
            yield return new WaitForSeconds(PowerUpRespawnDelay);
            collectable.SetActive(true);
        }
    }
}