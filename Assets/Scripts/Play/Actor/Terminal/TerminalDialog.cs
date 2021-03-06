﻿using System;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : Sébastien Arsenault
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