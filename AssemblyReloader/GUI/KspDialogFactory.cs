using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Annotations;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class KspDialogFactory : IDialogFactory
    {
        private readonly GUISkin _skin;

        public KspDialogFactory([NotNull] GUISkin skin)
        {
            if (skin == null) throw new ArgumentNullException("skin");
            _skin = skin;
        }


        public PopupDialog CreateMultiOptionDialog(
            string title, 
            string message,
            [NotNull] Callback drawMethod,
            [NotNull] params KeyValuePair<string, Callback>[] options)
        {
            if (drawMethod == null) throw new ArgumentNullException("drawMethod");
            if (options == null) throw new ArgumentNullException("options");

            return PopupDialog.SpawnPopupDialog(
                new MultiOptionDialog(message, drawMethod, title, _skin,
                    options.Select(kvp => new DialogOption(kvp.Key, kvp.Value, true)).ToArray()),
                    true, _skin);
        }
    }
}
