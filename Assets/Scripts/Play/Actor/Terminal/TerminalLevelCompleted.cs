using Harmony;

namespace Game
{
    //BR : Bon usage de l'héritage. Ici, on "ajoute" une fonctionalité à quelque chose
    //     d'existant. Il n'y a pas eu de fonctioanlité retirée. C'est ça le but de l'héritage
    //     en général : pas de remplacer le comportement par un autre, mais de l'étendre.

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