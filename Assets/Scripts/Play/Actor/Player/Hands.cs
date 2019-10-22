using UnityEngine;

namespace Game
{
    public class Hands : MonoBehaviour
    {
        private Box box;
        private bool isHoldingBox;

        public bool IsHoldingBox => isHoldingBox;

        private void Awake()
        {
            isHoldingBox = false;
        }

        public void Grab(Box box)
        {
            this.box = box;

            box.transform.SetParent(transform);
            box.Grabbed();
            isHoldingBox = true;
        }

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