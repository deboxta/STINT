using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public static class ComponentExtensions
    {
        /// <summary>
        ///   <para>Returns the component of Type type if the game object has one attached, throws an exception if it doesn't.</para>
        /// </summary>
        /// <param name="type">The type of Component to retrieve.</param>
        /// <returns>
        ///   <para>A component of the matching type, if found.</para>
        /// </returns>
        /// <exception cref="MissingComponentException">Thrown if the child component doesn't exist.</exception>
        public static T GetRequiredComponent<T>(this Component component)
        {
            var componentToFind = component.GetComponent<T>();
            if (componentToFind == null)
                throw new MissingComponentException(component.name + " is missing " + typeof(T).Name + " component.");
            return componentToFind;
        }

        /// <summary>
        ///   <para>
        ///     Returns the component of Type type in the GameObject or any of its children using depth first search,
        ///     throws an exception if the component doesn't exist in the children.
        ///   </para>
        /// </summary>
        /// <param name="t">The type of Component to retrieve.</param>
        /// <returns>
        ///   <para>A component of the matching type, if found.</para>
        /// </returns>
        /// <exception cref="MissingComponentException">Thrown if the child component doesn't exist.</exception>
        public static T GetRequiredComponentInChildren<T>(this Component component, bool includeInactive = false)
        {
            var childComponent = component.GetComponentInChildren<T>(includeInactive);
            if (childComponent == null)
                throw new MissingComponentException(component.name + " is missing "
                                                                   + typeof(T).Name + " child component.");
            return childComponent;
        }
    }
}