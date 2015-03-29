using System;
using ReeperCommon.Gui.Controls;

namespace AssemblyReloader.Gui
{
    public interface IExpandablePanelFactory
    {
        ICustomControl Create(string text, Action drawCall);
    }
}
