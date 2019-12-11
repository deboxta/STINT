using System;
using UnityEngine;

namespace Game
{
    [ObsoleteAttribute("This script is deprecated. Use " + nameof(SensorExistingCollider) + " instead.")]
    public sealed class SensorNewCollider : Sensor
    {
        [SerializeField] private Shape shape = Shape.Square;
        [SerializeField] [Range(1, 100)] private float xSize = 10;
        [SerializeField] [Range(1, 100)] private float ySize = 10;

        protected override void InitCollider()
        {
            switch (shape)
            {
                case Shape.Square:
                    var squareCollider = gameObject.AddComponent<BoxCollider2D>();
                    squareCollider.size = new Vector2(xSize, ySize);
                    collider2D = squareCollider;
                    break;
                case Shape.Circle:
                    var circleCollider = gameObject.AddComponent<CircleCollider2D>();
                    circleCollider.radius = xSize / 2;
                    collider2D = circleCollider;
                    break;
                default:
                    throw new Exception("Unknown shape named \"" + shape + "\".");
            }
        }
        
        private enum Shape
        {
            Square,
            Circle
        }
    }
}