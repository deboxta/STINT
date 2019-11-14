using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Jeammy Côté
    //BC : Wait...what ?
    //     Vous avez "CollectPowerUp" et "PowerUp". Il y a quelque chose qui marche pas. Structure à revoir.
    public class PowerUp : MonoBehaviour, ICollectible
    {
        //BC : Non respect des standards de nommage.
        [SerializeField] private float PowerUpRespawnDelay = 2;
        
        //BC : ??
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

        //BC : Private manquant.
        IEnumerator PowerUpSpawnTime()
        {
            yield return new WaitForSeconds(PowerUpRespawnDelay);
            collectable.SetActive(true);
        }
    }
}