using Harmony;
using UnityEngine;

namespace Game
{
    public abstract class Stimuli : MonoBehaviour
    {
        public event StimuliEventHandler OnDestroyed;

        protected virtual void Awake()
        {
            SetSensorLayer();
        }

        private void OnDestroy()
        {
            NotifyDestroyed();
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

    public delegate void StimuliEventHandler(GameObject otherObject);
}