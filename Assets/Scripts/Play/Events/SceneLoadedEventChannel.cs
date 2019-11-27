using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class SceneLoadedEventChannel : MonoBehaviour
    {
        public event SceneLoadedEventHandler OnSceneLoaded;
        
        public void NotifySceneLoaded()
        {
            if (OnSceneLoaded != null) OnSceneLoaded();
        }
        
        public delegate void SceneLoadedEventHandler();
    }
}