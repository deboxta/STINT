using System;
using Harmony;

namespace Game
{
    // Author : Mathieu Boutet
    public class ConstantLaser : Laser
    {
        private void Update()
        {
            laserBeam.SetPosition(0, LaserBeamStartPosition);
            laserBeam.SetPosition(1, LaserBeamEndPosition);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (CastTouchesPlayer)
                Finder.Player.Die();
        }
    }
}