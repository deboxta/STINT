using Harmony;

namespace Game
{
    // Legacy
    // Author : Mathieu Boutet
    public class ConstantLaser : Laser
    {
        private void Update()
        {
            laserBeamLineRenderer.SetPosition(0, LaserBeamStartPosition);
            laserBeamLineRenderer.SetPosition(1, LaserBeamEndPosition);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (CastTouchesPlayer)
                Finder.Player.Die();
        }
    }
}