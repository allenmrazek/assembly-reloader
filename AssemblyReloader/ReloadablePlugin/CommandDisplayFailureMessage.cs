using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandDisplayFailureMessage : Command
    {
        private readonly string _message;

        public CommandDisplayFailureMessage(string message)
        {
            if (message == null) throw new ArgumentNullException("message");

            _message = message;
        }

        public override void Execute()
        {
            PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), "Failure", _message, "Accept", true, HighLogic.UISkin);
        }
    }
}
