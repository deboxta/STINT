using UnityEngine;

namespace Game
{
    public class Treadmill : MonoBehaviour
    {
        [SerializeField] private float effectSpeed = 0.5f;

        private void OnCollisionStay2D(Collision2D other)
        {
            other.rigidbody.velocity = new Vector2(0, 0);

            var objectTrigger = other.gameObject;
            var position = objectTrigger.transform.position;
            position = new Vector3(position.x - effectSpeed, position.y, position.z);
            objectTrigger.transform.position = position;
        }
    }
}