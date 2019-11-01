using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public class DeadlyTrap : MonoBehaviour
    {
        private ISensor<Player> playerSensor;

        private void Awake()
        {
            playerSensor = GetComponentInChildren<Sensor>().For<Player>();
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