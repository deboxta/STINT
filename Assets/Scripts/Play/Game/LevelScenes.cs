using UnityEngine;

namespace Game
{
    public class LevelScenes : MonoBehaviour
    {
        [SerializeField] private string[] levelScenes;

        public string GetSceneName(int index)
        {
            return levelScenes[index];
        }
    }
}