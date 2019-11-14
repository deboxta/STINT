using Harmony;
using UnityEngine;

namespace Game
{
    public class SaveNamedBenSuccess : MonoBehaviour
    {
        public event SaveNamedBenSuccessEventHandler OnSaveNamedBen;

        private SavedDataLoadedEventChannel savedDataLoadedEventChannel;

        private void Awake()
        {
            savedDataLoadedEventChannel = Finder.SavedDataLoadedEventChannel;
        }

        private void OnEnable()
        {
            savedDataLoadedEventChannel.OnSavedDataLoaded += NotifySaveNamedBen;
        }

        private void OnDisable()
        {
            savedDataLoadedEventChannel.OnSavedDataLoaded -= NotifySaveNamedBen;
        }

        public void NotifySaveNamedBen() 
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