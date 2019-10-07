using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class FlashEffect : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int FLASH = Animator.StringToHash("Flash");

        public void Flash()
        {
            animator.SetTrigger(FLASH);
        }
    }
}