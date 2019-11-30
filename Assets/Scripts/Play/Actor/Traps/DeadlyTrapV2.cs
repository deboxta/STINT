using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public class DeadlyTrapV2 : MonoBehaviour
    {
        private ISensorV2<Player> playerSensor;

        private void Awake()
        {
            playerSensor = this.GetRequiredComponentInChildren<SensorV2>().For<Player>();
        }

        private void OnEnable()
        {
            playerSensor.OnSensedObject += OnPlayerSensed;
        }

        private void OnDisable()
        {
            playerSensor.OnSensedObject -= OnPlayerSensed;
        }

        private void OnPlayerSensed(Player otherObject)
        {
            otherObject.Die();
        }
    }
}