using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public static class ComponentExtensions
    {
        /// <summary>
        ///   <para>
        ///     Returns the component of Type type if the game object has one attached,
        ///     throws an exception if it doesn't.
        ///   </para>
        /// </summary>
        /// <returns>
        ///   <para>A component of the matching type, if found.</para>
        /// </returns>
        /// <exception cref="MissingComponentException">Thrown if the component to retrieve doesn't exist.</exception>
        public static T GetRequiredComponent<T>(this Component component)
        {
            var componentToFind = component.GetComponent<T>();
            if (componentToFind == null)
                throw new MissingComponentException(component.name + " is missing "
                                                                   + typeof(T).Name + " component.");
            return componentToFind;
        }

        /// <summary>
        ///   <para>
        ///     Returns the component of Type type in the GameObject or any of its children using depth first search,
        ///     throws an exception if the component doesn't exist in the children.
        ///   </para>
        /// </summary>
        /// <returns>
        ///   <para>A component of the matching type, if found.</para>
        /// </returns>
        /// <exception cref="MissingComponentException">Thrown if the component to retrieve doesn't exist in the children.</exception>
        public static T GetRequiredComponentInChildren<T>(this Component component, bool includeInactive = false)
        {
            var childComponent = component.GetComponentInChildren<T>(includeInactive);
            if (childComponent == null)
                throw new MissingComponentException(component.name + " is missing "
                                                                   + typeof(T).Name + " component in children.");
            return childComponent;
        }
        
        /// <summary>
        ///   <para>
        ///     Returns the component of Type type in the GameObject or any of its parents.,
        ///     throws an exception if the component doesn't exist in the parents.
        ///   </para>
        /// </summary>
        /// <returns>
        ///   <para>A component of the matching type, if found.</para>
        /// </returns>
        /// <exception cref="MissingComponentException">Thrown if the component to retrieve doesn't exist in the parents.</exception>
        public static T GetRequiredComponentInParent<T>(this Component component)
        {
            var childComponent = component.GetComponentInParent<T>();
            if (childComponent == null)
                throw new MissingComponentException(component.name + " is missing "
                                                                   + typeof(T).Name + " component in parents.");
            return childComponent;
        }
    }
}