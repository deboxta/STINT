using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class SecretRoomFoundEventChannel : MonoBehaviour
    {
        public event SecretRoomFoundEventHandler OnSecretRoomFound;
        
        public void NotifySecretRoomFound() 
        { 
            if (OnSecretRoomFound != null) OnSecretRoomFound();
        }
        
        public delegate void SecretRoomFoundEventHandler();
    }
}