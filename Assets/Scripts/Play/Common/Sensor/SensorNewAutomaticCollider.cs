using System;
using UnityEngine;

namespace Game
{
    public sealed class SensorNewAutomaticCollider : Sensor
    {
        [SerializeField] private Shape shape = Shape.Square;
        
        protected override void InitCollider()
        {
            switch (shape)
            {
                case Shape.Square:
                    collider2D = gameObject.AddComponent<BoxCollider2D>();
                    break;
                case Shape.Circle:
                    collider2D = gameObject.AddComponent<CircleCollider2D>();
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