using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public static class TransformExtensions
    {
        /// <summary>
        ///   <para>Finds a child by n in any child or sub child and returns it.</para>
        /// </summary>
        /// <param name="n">Name of child to be found.</param>
        /// <returns>
        ///   <para>The returned child transform or null if no child is found.</para>
        /// </returns>
        public static Transform FindAnywhere(this Transform transform, string n)
        {
            var allChildren = transform.GetComponentsInChildren<Transform>();
            foreach (var child in allChildren)
            {
                if(child.name == n)
                {
                    return child;
                }    
            }
            return null;
        }
    }
}