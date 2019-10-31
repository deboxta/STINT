using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class MenuPageChangedEventChannel : MonoBehaviour
    {
        public event OnPageChangedEventHandler OnPageChanged;
    
        public void NotifyPageChanged() 
        { 
            if (OnPageChanged != null) OnPageChanged();
        }
    
        public delegate void OnPageChangedEventHandler();
    }
}