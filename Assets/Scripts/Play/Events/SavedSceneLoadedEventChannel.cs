using Harmony;
using UnityEngine;

//Author : Yannick Cote
namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class SavedSceneLoadedEventChannel : MonoBehaviour
    {
        public event SavedSceneLoadedHandler OnSavedSceneLoaded;
        
        public void NotifySavedDataLoaded() 
        { 
            if (OnSavedSceneLoaded != null) OnSavedSceneLoaded();
        }
        
        public delegate void SavedSceneLoadedHandler();
    }
}