using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Jeammy Côté
    //BC : Nommage incorrect. Devrait être un nom (genre BootsPowerUp).
    public class CollectPowerUp : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //BC : Oh boy!
            //     Deux options s'offrent à vous :
            //         1. Faire une classe "PowerUp" contenant un SerializedField qui indique de quel "PowerUp" il s'agit (sous forme d'une enum).
            //         2. Faire deux classes : une "BootsPowerUp" et une "JumpPowerUp" qui s'occupent chacun de leur propre logique.
            //BC : Usage polymorphique de "ICollectible" devrait être fait ici.
            //     Cependant, le "PowerUp" devrait s'appliquer lui même sur le joueur, et non pas l'inverse.
            if (CompareTag(R.S.Tag.Collectable))
            {
                //BC : Devrait regarder si "other" est un player au lieu d'assumer que c'est le cas. Potentiel au bogue fort.
                Finder.Player.CollectPowerUp();
                var powerUp = GetComponentInParent<PowerUp>();
                if (powerUp != null)
                {
                    powerUp.Collect();
                }
            }
            else if (CompareTag(R.S.Tag.Boots))
            {
                Finder.Player.CollectBoots();
                var wallJumpBoots = GetComponentInParent<WallJumpBoots>();
                if (wallJumpBoots != null)
                {
                    wallJumpBoots.Collect();
                }
            }
        }
    }
}