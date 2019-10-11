using System;
using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(R.S.Tag.MainController)]
    public class FlashEffect : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private TimelineChangedEventChannel timelineChangedEventChannel;
        private static readonly int FLASH = Animator.StringToHash("Flash");

        private void Awake()
        {
            timelineChangedEventChannel = Finder.TimelineChangedEventChannel;
        }

        private void OnEnable()
        {
            timelineChangedEventChannel.OnTimelineChanged += TimelineChanged;
        }

        private void OnDisable()
        {
            timelineChangedEventChannel.OnTimelineChanged -= TimelineChanged;
        }

        private void TimelineChanged()
        {
            animator.SetTrigger(FLASH);
        }
    }
}