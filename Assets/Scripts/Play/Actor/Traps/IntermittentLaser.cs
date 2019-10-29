using System.Collections;
using UnityEngine;

namespace Game
{
    public class IntermittentLaser : ConstantLaser
    {
        [SerializeField] [Range(0, 100)] private float onTimeInSeconds = 1;
        [SerializeField] [Range(0, 100)] private float offTimeInSeconds = 1;

        private void Start()
        {
            StartCoroutine(SwitchLaserOnOff());
        }

        private IEnumerator SwitchLaserOnOff()
        {
            if (!Firing)
            {
                yield return new WaitForSeconds(offTimeInSeconds);
                Firing = true;
            }
            while (true)
            {
                yield return new WaitForSeconds(onTimeInSeconds);
                Firing = false;
                yield return new WaitForSeconds(offTimeInSeconds);
                Firing = true;
            }
        }
    }
}