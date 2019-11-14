using Harmony;
using UnityEngine;

namespace Game
{
    public class SaveNamedBenSuccess : MonoBehaviour
    {
        public event SaveNamedBenSuccessEventHandler OnSaveNamedBen;

        private SavedSceneLoadedEventChannel savedSceneLoadedEventChannel;

        private void Awake()
        {
            savedSceneLoadedEventChannel = Finder.SavedSceneLoadedEventChannel;
        }

        private void OnEnable()
        {
            savedSceneLoadedEventChannel.OnSavedSceneLoaded += NotifySaveNamedBen;
        }

        private void OnDisable()
        {
            savedSceneLoadedEventChannel.OnSavedSceneLoaded -= NotifySaveNamedBen;
        }

        private void NotifySaveNamedBen() 
        { 
            if (OnSaveNamedBen != null) OnSaveNamedBen();
        }

        public void DestroySuccess()
        {
            Destroy(this);
        }
        
        public delegate void SaveNamedBenSuccessEventHandler();
    }
}