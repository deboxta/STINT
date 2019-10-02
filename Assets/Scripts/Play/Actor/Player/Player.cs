using Harmony;

namespace Game
{
    public class Player : Actor
    {
        private PlayerHitEventChannel playerHitEventChannel;
        private PlayerDeathEventChannel playerDeathEventChannel;

        private void Awake()
        {
            playerHitEventChannel = Finder.PlayerHitEventChannel;
            playerDeathEventChannel = Finder.PlayerDeathEventChannel;
        }

        private void OnEnable()
        {
            playerHitEventChannel.OnPlayerHit += Hit;
        }

        private void OnDisable()
        {
            playerHitEventChannel.OnPlayerHit -= Hit;
        }

        private void Hit()
        {
            Die();
        }

        private void Die()
        {
            playerDeathEventChannel.NotifyPlayerDeath();
        }
    }
}