using Harmony;

namespace Game
{
    // Author : Mathieu Boutet
    public class ConstantLaser : Laser
    {
        private void Update()
        {
            //BC : Devrait se faire dans la classe parent. Je vois vraiment pas pourquoi tu fais cela là.
            laserBeam.SetPosition(0, LaserBeamStartPosition);
            laserBeam.SetPosition(1, LaserBeamEndPosition);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            //BC : Événement devrait être déclanché dans la classe parent, et non pas ici.
            //     Responsabilité au mauvais endroit.
            if (CastTouchesPlayer)
                Finder.Player.Die();
        }
    }
}