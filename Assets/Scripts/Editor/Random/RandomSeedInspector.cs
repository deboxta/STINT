using Harmony;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(RandomSeed), true)]
    public class RandomSeedInspector : BaseInspector
    {
        private RandomSeed randomSeed;

        protected override void Initialize()
        {
            randomSeed = target as RandomSeed;
        }

        protected override void Draw()
        {
            Initialize();
            DrawDefault();

            if (EditorApplication.isPlaying)
            {
                DrawInfoBox("Actual seed : " + randomSeed.Seed);
            }
            else
            {
                DrawWarningBox("Actual seed can only be seen in play mode.");
            }
        }
    }
}