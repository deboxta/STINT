
using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class FlashEffect : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int FADE_OUT = Animator.StringToHash("Fade-out");
        
        
    }
}