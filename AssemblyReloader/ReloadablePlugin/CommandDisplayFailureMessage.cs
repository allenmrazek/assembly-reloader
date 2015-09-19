extern alias KSP;
using System;
using strange.extensions.command.impl;
using HighLogic = KSP::HighLogic;

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
            KSP::PopupDialog.SpawnPopupDialog("Failure", _message, "Accept", true, HighLogic.Skin);
        }
    }
}
