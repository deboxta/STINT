using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class LevelScenes : MonoBehaviour
    {
        [SerializeField] private string[] levelScenes;

        public string GetSceneName(int index)
        {
            return levelScenes[index];
        }
    }
}