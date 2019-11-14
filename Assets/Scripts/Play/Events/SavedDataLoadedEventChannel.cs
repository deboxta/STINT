using Harmony;
using UnityEngine;

//Author : Yannick Cote
namespace Game
{
    [Findable(R.S.Tag.MainController)]

    public class SavedDataLoadedEventChannel : MonoBehaviour
    {
        public event SavedDataLoadedHandler OnSavedDataLoaded;
        
        public void NotifySavedDataLoaded() 
        { 
            if (OnSavedDataLoaded != null) OnSavedDataLoaded();
        }
        
        public delegate void SavedDataLoadedHandler();
    }
}