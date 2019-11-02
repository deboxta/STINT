using System.Timers;

namespace Game
{
    public class PausableTimer : Timer
    {
        private double timeLeft;

        public void Pause()
        {
            timeLeft = Interval;
            Stop();
        }

        public void Resume()
        {
            Interval = timeLeft;
            Start();
        }
    }
}