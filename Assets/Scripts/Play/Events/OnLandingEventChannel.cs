using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class OnLandingEventChannel : MonoBehaviour
    {
        public event OnLandingEventHandler Onlanding;

        public void NotifyOnLanding()
        {
            Onlanding?.Invoke();
        }
        
        public delegate void OnLandingEventHandler();
    }
}