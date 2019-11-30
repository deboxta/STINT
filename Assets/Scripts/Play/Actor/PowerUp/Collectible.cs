using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Jeammy Côté
    public class Collectible : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
    }
}