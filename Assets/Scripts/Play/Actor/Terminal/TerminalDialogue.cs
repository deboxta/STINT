using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    public class TerminalDialogue : Terminal
    {
        //BR : Vous auriez tout à gagner à faire des "ScriptableObjects" pour le "Data"
        //     de vos dialogues.
        //     Me voir au besoin.
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