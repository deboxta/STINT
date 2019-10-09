using UnityEngine;

namespace Game
{
    public class Box : MonoBehaviour
    {
        [SerializeField] private int throwedForceUp = 40;
        [SerializeField] private int throwedForceX = 25;

    private Rigidbody2D rigidbody2D;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Grabbed()
        {
            rigidbody2D.simulated = false;
            transform.localPosition = Vector3.zero;
        }

        public void Throwed(bool isLookingRight)
        {
            transform.parent = null;
            rigidbody2D.simulated = true;
            rigidbody2D.velocity = new Vector2(throwedForceX, throwedForceUp);
            if (!isLookingRight)
            {
                rigidbody2D.velocity = new Vector2(-throwedForceX, throwedForceUp);
            }
        }
    }
}

