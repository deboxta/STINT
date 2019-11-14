using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class Scenes : MonoBehaviour
    {
        [SerializeField] private string[] scenes;

        public string GetSceneName(int index)
        {
            return scenes[index];
        }
    }
}