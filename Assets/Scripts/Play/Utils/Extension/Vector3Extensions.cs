using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public static class Vector3Extensions
    {
        /// <summary>
        /// Checks if the point is before another point in the specified direction.
        /// </summary>
        public static bool IsBefore(this Vector3 point1, Vector3 point2, Vector3 direction, float precision = 1f)
        {
            return point2.IsPast(point1, direction, precision);
        }
        
        /// <summary>
        /// Checks if the point is past another point in the specified direction.
        /// </summary>
        public static bool IsPast(this Vector3 point1, Vector3 point2, Vector3 direction, float precision = 1f)
        {
            return point2.DirectionTo(point1).IsSameDirection(direction, precision);
        }
    }
}