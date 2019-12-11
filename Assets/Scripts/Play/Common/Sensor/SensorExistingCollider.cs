using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class SensorExistingCollider : Sensor
    {
        protected override void InitCollider()
        {
            collider2D = gameObject.GetComponent<Collider2D>();
        }
    }
}