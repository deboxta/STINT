using System;
using Harmony;
using UnityEngine;

namespace Game
{
    public sealed class Stimuli : MonoBehaviour
    {
        [SerializeField] private Shape shape = Shape.Sphere;
        [SerializeField] [Range(1, 100)] private float size = 1f;

        public event StimuliEventHandler OnDestroyed;

        private void Awake()
        {
            CreateCollider();
            SetSensorLayer();
        }

        private void OnDestroy()
        {
            NotifyDestroyed();
        }

        private void CreateCollider()
        {
            switch (shape)
            {
                case Shape.Cube:
                    var cubeCollider = gameObject.AddComponent<BoxCollider>();
                    cubeCollider.isTrigger = true;
                    cubeCollider.size = Vector3.one * size;
                    break;
                case Shape.Sphere:
                    var sphereCollider = gameObject.AddComponent<SphereCollider>();
                    sphereCollider.isTrigger = true;
                    sphereCollider.radius = size / 2;
                    break;
                default:
                    throw new Exception("Unknown shape named \"" + shape + "\".");
            }
        }

        private void SetSensorLayer()
        {
            gameObject.layer = LayerMask.NameToLayer(R.S.Layer.Sensor);
        }

        private void NotifyDestroyed()
        {
            if (OnDestroyed != null) OnDestroyed(transform.parent.gameObject);
        }

        private enum Shape
        {
            Cube,
            Sphere
        }
    }

    public delegate void StimuliEventHandler(GameObject otherObject);
}