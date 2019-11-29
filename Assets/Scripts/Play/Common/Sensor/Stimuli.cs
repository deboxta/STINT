using System;
using Harmony;
using UnityEngine;

namespace Game
{
    [ObsoleteAttribute("The old sensor is deprecated. Use " + nameof(StimuliV2) + " instead.")]
    public sealed class Stimuli : MonoBehaviour
    {
        [SerializeField] private Shape shape = Shape.Circle;
        [SerializeField] [Range(1, 100)] private float xSize = 10;
        [SerializeField] [Range(1, 100)] private float ySize = 10;

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
                case Shape.Square:
                    var squareCollider = gameObject.AddComponent<BoxCollider2D>();
                    squareCollider.isTrigger = true;
                    squareCollider.size = new Vector2(xSize, ySize);
                    break;
                case Shape.Circle:
                    var circleCollider = gameObject.AddComponent<CircleCollider2D>();
                    circleCollider.isTrigger = true;
                    circleCollider.radius = xSize / 2;
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
            Square,
            Circle
        }
    }

    public delegate void StimuliEventHandler(GameObject otherObject);
}