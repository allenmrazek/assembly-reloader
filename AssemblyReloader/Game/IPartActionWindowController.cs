using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Game
{
    public interface IPartActionWindowController
    {
        void Close();
        void Add(UIPartActionWindow window);
        void Remove(UIPartActionWindow window);
        void Refresh();
    }
}
