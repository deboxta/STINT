using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Yannick Cote
    [Findable(R.S.Tag.MainController)]
    public class CameraShakeEnterEventChannel : MonoBehaviour
    {
        public event ShakeCameraEventHandler OnShakeEnter;
        
        public void NotifyCameraShake() 
        { 
            if (OnShakeEnter != null) OnShakeEnter();
        }
        
        public delegate void ShakeCameraEventHandler();
    }
}