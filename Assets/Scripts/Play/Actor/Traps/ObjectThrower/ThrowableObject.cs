using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public class ThrowableObject : MonoBehaviour, IFreezable
    {
        [Range(0, 1)] [SerializeField] private float frictionWithPlayer = 0.75f;

        private TimeFreezeEventChannel timeFreezeEventChannel;
        private Vector2 lastFixedUpdateVelocity;
        private FreezableWaitForSeconds waitForRemoveObjectDelay;
        private BoxCollider2D boxCollider2D;

        private static bool IsFrozen => Finder.TimeFreezeController.IsFrozen;
        public Rigidbody2D Rigidbody2D { get; private set; }
        public ObjectThrower Thrower { get; set; }

        private ContactPoint2D[] colliderContactPoints;

        private void Awake()
        {
            timeFreezeEventChannel = Finder.TimeFreezeEventChannel;
            // Needs to be subscribed at the beginning and never unsubscribed
            // because the state has to update even when the object is disabled
            timeFreezeEventChannel.OnTimeFreezeStateChanged += OnTimeFreezeStateChanged;
            Rigidbody2D = this.GetRequiredComponent<Rigidbody2D>();
            boxCollider2D = this.GetRequiredComponentInChildren<BoxCollider2D>();
        }

        private void Start()
        {
            Reset();
        }

        private void OnEnable()
        {
            StartCoroutine(RemoveObjectAfterDelay());
            Rigidbody2D.velocity = lastFixedUpdateVelocity;
        }

        private void FixedUpdate()
        {
            if (!IsFrozen)
                Rigidbody2D.velocity += Physics2D.gravity * Time.fixedDeltaTime;

            // Velocity needs to be saved here instead of in OnDisable because when changing timelines,
            // the Rigidbody is disabled before the script so the velocity is always 0 in OnDisable
            // (velocity is reset automatically on a Rigidbody when it's disabled).
            lastFixedUpdateVelocity = Rigidbody2D.velocity;
        }

        private void OnDestroy()
        {
            timeFreezeEventChannel.OnTimeFreezeStateChanged -= OnTimeFreezeStateChanged;
        }

        public void Reset()
        {
            waitForRemoveObjectDelay = new FreezableWaitForSeconds(Thrower.removeObjectDelay);
        }

        private IEnumerator RemoveObjectAfterDelay()
        {
            yield return waitForRemoveObjectDelay;
            Thrower.RemoveThrownObject(this);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
#if UNITY_EDITOR
            colliderContactPoints = other.contacts;
#endif
            if (!IsFrozen&& other.transform.parent != null && other.transform.parent.CompareTag(R.S.Tag.Player))
            {
                Finder.Player.PlayerMover.YVelocityToLerp = Rigidbody2D.velocity.y;

                var contactPointsDistance = other.GetContact(0).point.DistanceTo(other.GetContact(1).point);
                Finder.Player.PlayerMover.YVelocityLerpTValue
                    = contactPointsDistance / boxCollider2D.size.y * frictionWithPlayer;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
#if UNITY_EDITOR
            colliderContactPoints = null;
#endif
            if (!IsFrozen && other.transform.parent != null && other.transform.CompareTag(R.S.Tag.Player))
            {
                Finder.Player.PlayerMover.YVelocityToLerp = null;
            }
        }

        private void OnTimeFreezeStateChanged()
        {
            enabled = !IsFrozen;
            if (IsFrozen)
            {
                Rigidbody2D.velocity = Vector2.zero;
                Finder.Player.PlayerMover.YVelocityToLerp = null;
            }
            else
            {
                Rigidbody2D.velocity = lastFixedUpdateVelocity;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Collider hit points
            if (colliderContactPoints != null && colliderContactPoints.Length > 0)
            {
                foreach (var contactPoint in colliderContactPoints)
                {
                    Gizmos.DrawSphere(contactPoint.point, 0.1f);
                }
            }

            // Reset color
            Gizmos.color = Color.white;
        }
#endif
    }
}