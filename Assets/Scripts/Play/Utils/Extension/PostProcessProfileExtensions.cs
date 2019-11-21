using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Game
{
    public static class PostProcessProfileExtensions
    {
        public static PostProcessProfile Clone(this PostProcessProfile profile)
        {
            PostProcessProfile profileClone = ScriptableObject.CreateInstance<PostProcessProfile>();
            foreach (var setting in profile.settings)
            {
                var settingClone = Object.Instantiate(setting);
                profileClone.settings.Add(settingClone);
            }
            return profileClone;
        }
    }
}