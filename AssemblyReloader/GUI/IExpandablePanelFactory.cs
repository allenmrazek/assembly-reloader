using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Gui.Controls;

namespace AssemblyReloader.GUI
{
    public interface IExpandablePanelFactory
    {
        ICustomControl Create(string text, Action drawCall);
    }
}
