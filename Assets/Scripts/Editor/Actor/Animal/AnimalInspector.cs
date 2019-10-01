using Harmony;
using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(Animal), true)]
    public class AnimalInspector : BaseInspector
    {
        private Animal animal;

        protected override void Initialize()
        {
            animal = target as Animal;
        }

        protected override void Draw()
        {
            Initialize();
            DrawDefault();
            
            if (EditorApplication.isPlaying && animal.Vitals != null) //Check if playing and is not prefab
            {
                BeginTable();
                DrawVitals();
                EndTable();
            }
            else
            {
                DrawWarningBox("Animal information can only be seen on real animal (not prefab) and only in play mode.");
            }
        }

        private void DrawVitals()
        {
            BeginTableCol();
            BeginTableRow();
            DrawTableHeader("Vitals");
            EndTableRow();
            BeginTableRow();
            DrawTableCell("Hunger : " + Mathf.RoundToInt(animal.Vitals.Hunger * 100) + "%");
            EndTableRow();
            BeginTableRow();
            DrawTableCell("Thirst : " + Mathf.RoundToInt(animal.Vitals.Thirst * 100) + "%");
            EndTableRow();
            BeginTableRow();
            DrawTableCell("ReproductiveUrge : " + Mathf.RoundToInt(animal.Vitals.ReproductiveUrge * 100) + "%");
            EndTableRow();
            EndTableCol();
        }
    }
}