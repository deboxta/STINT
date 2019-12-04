using UnityEngine;

namespace Game
{
    //Author : Jeammy Côté
    public class WallJumpBoots : MonoBehaviour, ICollectible
    {
        public void Collect()
        {
            Destroy(gameObject);
        }
    }
}