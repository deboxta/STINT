using Harmony;
using UnityEngine;

namespace Game
{
    public class SecretRoomFoundSuccess : MonoBehaviour, ISuccess
    {
        public event SecretRoomFoundEventChannel.SecretRoomFoundEventHandler OnSecretRoomFound;
        
        private SecretRoomFoundEventChannel secretRoomFoundEventChannel;
        
        public string successName { get; set; }
        
        private void Awake()
        {
            secretRoomFoundEventChannel = Finder.SecretRoomFoundEventChannel;
            
            successName = "SecretRoomFoundSuccess";
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