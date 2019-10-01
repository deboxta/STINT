using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace Game
{
    public static class EntityExtensions
    {
        public static T FindNearest<T>(this IEnumerable<T> list, Vector3 position, float maxDistance = float.MaxValue) where T : class, IEntity
        {
            maxDistance *= maxDistance;
            
            T closest = null;
            var closestSqrDistance = float.MaxValue;
            foreach (var element in list)
            {
                var eatablePosition = element.Position;
                var sqrDistance = position.SqrDistanceTo(eatablePosition);

                if (sqrDistance < maxDistance && sqrDistance < closestSqrDistance)
                {
                    closest = element;
                    closestSqrDistance = sqrDistance;
                }
            }

            return closest;
        }
    }
}