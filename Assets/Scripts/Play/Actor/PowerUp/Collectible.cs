using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Jeammy Côté
    //TODO : Change with inheritance
    public class Collectible : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
    }
}