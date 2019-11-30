using UnityEngine;

namespace Game
{
    //Author : Anthony Bérubé
    
    public class Hands : MonoBehaviour
    {
        private Box box;

        public bool IsHoldingBox => box != null;
        

        public void Grab(Box box)
        {
            this.box = box;

            box.transform.SetParent(transform);
            box.Grab();
        }

        public void Throw(bool isLookingRight)
        {
            box.Throw(isLookingRight);
            box = null;
        }

        public void Drop()
        {
            box.Drop();
            box = null;
        }
    }
}