using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public sealed class StimuliV2 : MonoBehaviour
    {
        public event StimuliV2EventHandler OnDestroyed;

        private void Awake()
        {
            InitCollider();
            SetSensorLayer();
        }

        private void OnDestroy()
        {
            NotifyDestroyed();
        }

        private void InitCollider()
        {
            gameObject.GetComponent<Collider2D>().isTrigger = true;
        }

        private void SetSensorLayer()
        {
            gameObject.layer = LayerMask.NameToLayer(R.S.Layer.Sensor);
        }

        private void NotifyDestroyed()
        {
            if (OnDestroyed != null) OnDestroyed(transform.parent.gameObject);
        }
    }

    public delegate void StimuliV2EventHandler(GameObject otherObject);
}