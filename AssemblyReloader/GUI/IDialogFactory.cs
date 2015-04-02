using System.Collections.Generic;

namespace AssemblyReloader.Gui
{
    public interface IDialogFactory
    {
        PopupDialog CreateMultiOptionDialog(string title, string message, Callback drawMethod, params KeyValuePair<string, Callback>[] options);
    }
}
