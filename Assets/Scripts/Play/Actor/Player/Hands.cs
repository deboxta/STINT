using UnityEngine;

namespace Game
{
    //Author : Anthony Bérubé
    
    //BR : Pas loin d'être propre. Quelques modifications à faire seulement.
    
    public class Hands : MonoBehaviour
    {
        private Box box;
        private bool isHoldingBox;

        public bool IsHoldingBox => isHoldingBox;

        private void Awake()
        {
            //BC : Duplication d'information. Si "box" est pas null, c'est que tu "hold" une box.
            //     Je me permet de pénaliser ici, car c'était très facile à régler.
            //     Éviter de faire cela à l'avenir : c'est une source importante de bogues.
            isHoldingBox = false;
        }

        public void Grab(Box box)
        {
            this.box = box;

            box.transform.SetParent(transform);
            box.Grabbed();
            isHoldingBox = true;
        }

        //BC : Voir commentaires dans "Box" sur la méthode "Throwed". C'est la même chose ici.
        public void Throw(bool isLookingRight)
        {
            box.Throwed(isLookingRight);
            box = null;
            isHoldingBox = false;
        }

        public void Drop()
        {
            box.Dropped();
            box = null;
            isHoldingBox = false;
        }
    }
}