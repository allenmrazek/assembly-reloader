using System;
using ReeperCommon.Gui.Controls;

namespace AssemblyReloader.GUI
{
    public interface IExpandablePanelFactory
    {
        ICustomControl Create(string text, Action drawCall);
    }
}
