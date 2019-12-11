using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public class ConveyorBelt : MonoBehaviour, IFreezable
    {
        [SerializeField] private float effectSpeed = 0.5f;
        [SerializeField] private bool canBeFrozen = true;

        private new Collider2D collider2D;
        
        private static bool IsFrozen => Finder.TimeFreezeController.IsFrozen;

        private void Awake()
        {
            collider2D = this.GetRequiredComponentInChildren<Collider2D>();
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (IsFrozen && canBeFrozen)
            {
                collider2D.gameObject.layer = LayerMask.NameToLayer(R.S.Layer.Floor);
            }
            else
            {
                collider2D.gameObject.layer = LayerMask.NameToLayer(R.S.Layer.Default);
                other.transform.Translate(transform.right * effectSpeed);
            }
        }
    }
}