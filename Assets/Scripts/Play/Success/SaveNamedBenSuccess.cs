using Harmony;
using UnityEngine;

namespace Game
{
    public class SaveNamedBenSuccess : MonoBehaviour, ISuccess
    {
        public event SaveNamedBenSuccessEventHandler OnSaveNamedBen;

        private SavedSceneLoadedEventChannel savedSceneLoadedEventChannel;
        
        public string successName { get; set; }

        private void Awake()
        {
            savedSceneLoadedEventChannel = Finder.SavedSceneLoadedEventChannel;
            
            successName = "Save Named Ben Success";
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