using UnityEngine;

namespace Game
{
    public abstract class Actor : MonoBehaviour, IEntity
    {
        public Vector3 Position => transform.position;
    }
}