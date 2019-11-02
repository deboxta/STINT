using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class TimeFreezeController : MonoBehaviour
    {
        private bool isFrozen;
        private TimeFreezeEventChannel timeFreezeEventChannel;
        
        public bool IsFrozen
        {
            get => isFrozen;
            private set
            {
                isFrozen = value;
                timeFreezeEventChannel.NotifyTimeFreezeStateChanged();
            }
        }

        private void Awake()
        {
            timeFreezeEventChannel = Finder.TimeFreezeEventChannel;
        }

        private void Start()
        {
            isFrozen = false;
        }
        
        public void Reset()
        {
            isFrozen = false;
        }

        public void SwitchState()
        {
            IsFrozen = !IsFrozen;
        }
    }
}