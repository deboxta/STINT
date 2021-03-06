﻿using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Jeammy Côté
    public class PowerUp : MonoBehaviour, ICollectible
    {
        [SerializeField] private float powerUpRespawnDelay = 2;
        
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

        private IEnumerator PowerUpSpawnTime()
        {
            yield return new WaitForSeconds(powerUpRespawnDelay);
            collectable.SetActive(true);
        }
    }
}