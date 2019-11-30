using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public class IntermittentLaser : ConstantLaser, IFreezable
    {
        [Range(0, 100)] [SerializeField] private float onTimeInSeconds = 1;
        [Range(0, 100)] [SerializeField] private float offTimeInSeconds = 1;
        [SerializeField] private bool isOnAtStart = true;
        
        // By using the same instance every time, the time left stays the same if the
        // coroutine is recreated, and it uses object recycling at the same time.
        private FreezableWaitForSeconds waitForChangeFiringStateDelay;
        private bool firing;

        public bool IsFrozen => Finder.TimeFreezeController.IsFrozen;

        private bool Firing
        {
            get => firing;
            set
            {
                firing = value;
                if (!value)
                    laserBeamLineRenderer.SetPosition(1, transform.position);
                laserBeamLineRenderer.gameObject.SetActive(value);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            firing = isOnAtStart;
            if (isOnAtStart)
                waitForChangeFiringStateDelay = new FreezableWaitForSeconds(onTimeInSeconds);
            else
                waitForChangeFiringStateDelay = new FreezableWaitForSeconds(offTimeInSeconds);
        }

        private void OnEnable()
        {
            StartCoroutine(SwitchFiringStateAtInterval());
        }

        protected override void FixedUpdate()
        {
            if (Firing) 
                base.FixedUpdate();
        }

        private IEnumerator SwitchFiringStateAtInterval()
        {
            // If it starts off, invert it at the beginning instead of checking a condition every loop
            if (!Firing)
            {
                yield return waitForChangeFiringStateDelay;
                Firing = true;
                waitForChangeFiringStateDelay.Reset(onTimeInSeconds);
            }
            while (true)
            {
                yield return waitForChangeFiringStateDelay;
                Firing = false;
                waitForChangeFiringStateDelay.Reset(offTimeInSeconds);
                yield return waitForChangeFiringStateDelay;
                Firing = true;
                waitForChangeFiringStateDelay.Reset(onTimeInSeconds);
            }
        }
    }
}