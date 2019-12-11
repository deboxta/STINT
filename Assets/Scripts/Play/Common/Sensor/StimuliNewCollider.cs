using System;
using UnityEngine;

namespace Game
{
    [ObsoleteAttribute("This script is deprecated. Use " + nameof(StimuliExistingCollider) + " instead.")]
    public sealed class StimuliNewCollider : Stimuli
    {
        [SerializeField] private Shape shape = Shape.Circle;
        [SerializeField] [Range(1, 100)] private float xSize = 10;
        [SerializeField] [Range(1, 100)] private float ySize = 10;

        protected override void Awake()
        {
            base.Awake();
            
            CreateCollider();
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

        private enum Shape
        {
            Square,
            Circle
        }
    }
}