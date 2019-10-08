using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class LevelArray : MonoBehaviour
    {
        [SerializeField] private string[] levelsArray;

        public string GetSceneName(int index)
        {
            return levelsArray[index];
        }
    }
}