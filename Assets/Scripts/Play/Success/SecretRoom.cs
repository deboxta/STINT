using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class SecretRoom : MonoBehaviour
    {
        private ISensor<Player> playerSensor;
        private SecretRoomFoundEventChannel secretRoomFoundEventChannel;

        private void Awake()
        {
            playerSensor = GetComponent<Sensor>().For<Player>();
            secretRoomFoundEventChannel = Finder.SecretRoomFoundEventChannel;
        }

        private void OnEnable()
        {
            playerSensor.OnSensedObject += OnPlayerSensed;
        }

        private void OnPlayerSensed(Player otherobject)
        {
            secretRoomFoundEventChannel.NotifySecretRoomFound();
        }
    }
}