using Harmony;
using UnityEngine;

namespace Game
{
    public class SecretRoomFoundSuccess : MonoBehaviour
    {
        public event SecretRoomFoundEventChannel.SecretRoomFoundEventHandler OnSecretRoomFound;
        
        private SecretRoomFoundEventChannel secretRoomFoundEventChannel;
        
        private void Awake()
        {
            secretRoomFoundEventChannel = Finder.SecretRoomFoundEventChannel;
        }

        private void OnEnable()
        {
            secretRoomFoundEventChannel.OnSecretRoomFound += NotifySecretRoomFound;
        }

        private void OnDisable()
        {
            secretRoomFoundEventChannel.OnSecretRoomFound -= NotifySecretRoomFound;
        }

        private void NotifySecretRoomFound() 
        { 
            if (OnSecretRoomFound != null) OnSecretRoomFound();
        }

        public void DestroySuccess()
        {
            Destroy(this);
        }
        
        public delegate void SaveNamedBenSuccessEventHandler();
    }
}