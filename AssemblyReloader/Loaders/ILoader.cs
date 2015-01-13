using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Messages;
using AssemblyReloader.Messages.Implementation;

namespace AssemblyReloader.Loaders
{
    interface ILoader : IDisposable
    {
        void Initialize();
        void Deinitialize();
    }
}
