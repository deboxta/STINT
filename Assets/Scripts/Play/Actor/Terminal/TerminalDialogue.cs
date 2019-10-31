using Harmony;
using UnityEngine;

namespace Game
{
    public class TerminalDialogue : Terminal
    {
        [SerializeField] private string[] texts;
        private HudDialogue hudDialogue;
        
        protected override void Awake()
        {
            base.Awake();

            hudDialogue = Finder.HudDialogue;
        }

        protected override void OnPlayerSensed(Player player)
        {
            base.OnPlayerSensed(player);
            
            hudDialogue.StartDisplaying(texts);
        }
    }
}