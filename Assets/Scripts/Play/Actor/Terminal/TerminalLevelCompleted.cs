using System;
using Harmony;

namespace Game
{
    //Author : Sébastien Arsenault
    public class TerminalLevelCompleted : Terminal
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