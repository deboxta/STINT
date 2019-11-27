using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    [Findable(R.S.Tag.GameController)]
    public class TimeFreezeController : MonoBehaviour
    {
        [SerializeField] private bool startFrozen = false;
        [SerializeField] private bool changeStateAtInterval = false;
        [SerializeField] private float changeStateDelay = 5;
        [SerializeField] private float initialChangeStateDelay = 2;
        
        private bool isFrozen;
        private TimeFreezeEventChannel timeFreezeEventChannel;

        public bool IsFrozen
        {
            get => isFrozen;
            private set
            {
                isFrozen = value;
#if UNITY_EDITOR
                Debug.Log("Time frozen: " + value);
#endif
                timeFreezeEventChannel.NotifyTimeFreezeStateChanged();
            }
        }

        private void Awake()
        {
            timeFreezeEventChannel = Finder.TimeFreezeEventChannel;
            isFrozen = startFrozen;
        }

        private void OnEnable()
        {
            if (changeStateAtInterval)
                StartCoroutine(WaitForInitialDelay());
        }

        public void Reset()
        {
            isFrozen = false;
        }

        public void SwitchState()
        {
            IsFrozen = !IsFrozen;
        }
        
        private IEnumerator WaitForInitialDelay()
        {
            yield return new WaitForSeconds(initialChangeStateDelay);
            SwitchState();
            StartCoroutine(SwitchStateAtInterval());
        }

        private IEnumerator SwitchStateAtInterval()
        {
            while (true)
            {
                yield return new WaitForSeconds(changeStateDelay);
                SwitchState();
            }
        }
    }
}