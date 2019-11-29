using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public static class Vector2Extensions
    {
        /// <summary>
        /// Checks if the point is before another point in the specified direction.
        /// </summary>
        public static bool IsBefore(this Vector2 point1, Vector2 point2, Vector2 direction, float precision = 1f)
        {
            return point2.IsPast(point1, direction, precision);
        }
        
        /// <summary>
        /// Checks if the point is past another point in the specified direction.
        /// </summary>
        public static bool IsPast(this Vector2 point1, Vector2 point2, Vector2 direction, float precision = 1f)
        {
            return point2.DirectionTo(point1).IsSameDirection(direction, precision);
        }
    }
}