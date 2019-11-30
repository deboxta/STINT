using Harmony;

namespace Game
{
    //Author : Sébastien Arsenault
    public class TerminalLevelCompletedV2 : TerminalV2
    {
        private LevelCompletedEventChannel levelCompletedEventChannel;
        
        protected override void Awake()
        {
            base.Awake();
            
            levelCompletedEventChannel = Finder.LevelCompletedEventChannel;
        }

        protected override void OnPlayerSensed(Player player)
        {
            base.OnPlayerSensed(player);
            
            levelCompletedEventChannel.NotifyLevelCompleted();
        }
    }
}