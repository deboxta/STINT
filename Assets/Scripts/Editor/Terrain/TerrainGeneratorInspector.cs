using Harmony;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(TerrainGridGenerator), true)]
    public class TerrainGeneratorInspector : BaseInspector
    {
        protected override void Initialize()
        {
            //Empty
        }

        protected override void Draw()
        {
            Initialize();
            DrawDefault();

            DrawInfoBox("Everything above Sand level is considered Grass.");
        }
    }
}