using Harmony;
using UnityEngine;

namespace Game
{
    public struct LoseReproductiveUrge : IEffect
    {
        public void ApplyOn(GameObject gameObject)
        {
            gameObject.SendMessageToChild<VitalStats>(it => it.HaveSex());
        }
    }
}