using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Messages;
using AssemblyReloader.Messages.Implementation;

namespace AssemblyReloader.Loaders
{
    interface ILoader
    {
        void Initialize();
        void Deinitialize();
    }
}
