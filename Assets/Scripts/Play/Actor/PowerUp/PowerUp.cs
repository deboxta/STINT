using Game;
using Harmony;
using UnityEngine;

namespace Game
{
    public class PowerUp : MonoBehaviour, Collectible
    {
        public void Collect()
        {
            Destroy(this.Root());
        }
    }
}