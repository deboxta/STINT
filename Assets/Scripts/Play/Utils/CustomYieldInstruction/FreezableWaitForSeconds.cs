using Harmony;
using UnityEngine;

namespace Game
{
    // Author : Mathieu Boutet
    public class FreezableWaitForSeconds : CustomYieldInstruction
    {
        public float TimeToWait { get; set; }
        public float TimeLeft { get; private set; }
        public override bool keepWaiting
        {
            get
            {
                if (!Finder.TimeFreezeController.IsFrozen)
                    TimeLeft -= Time.deltaTime;
                return TimeLeft > 0;
            }
        }
        
        /// <summary>
        ///   <para>
        ///     Suspends the coroutine execution for the given amount of
        ///     seconds using scaled time and is affected by time freeze.
        ///   </para>
        /// </summary>
        public FreezableWaitForSeconds(float seconds)
        {
            TimeToWait = seconds;
            Reset();
        }
        
        /// <summary>
        /// Resets the time left to the initial time.
        /// </summary>
        public new void Reset()
        {
            Reset(TimeToWait);
        }
        
        /// <summary>
        /// Resets the time left to the number of seconds specified.
        /// </summary>
        public void Reset(float seconds)
        {
            TimeLeft = seconds;
        }
    }
}