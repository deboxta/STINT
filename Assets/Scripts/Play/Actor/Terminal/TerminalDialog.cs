using System;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
    [Obsolete("The old sensor is deprecated. Use " + nameof(TerminalDialogV2) + " instead.")]
    public class TerminalDialog : Terminal
    {
        [SerializeField] private string[] texts;
        private HudDialog hudDialog;
        
        protected override void Awake()
        {
            base.Awake();

            hudDialog = Finder.HudDialog;
        }

        protected override void OnPlayerSensed(Player player)
        {
            base.OnPlayerSensed(player);
            
            hudDialog.StartDisplaying(texts);
        }
    }
}