using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public static class MonoBehaviourExtensions
    {
        public static IEnumerator FreezableWaitForSeconds(this MonoBehaviour monoBehaviour, float seconds)
        {
            while (seconds > 0)
            {
                if (!Finder.TimeFreezeController.IsFrozen)
                    seconds -= Time.deltaTime;
                yield return seconds;
            }
        }
    }
}