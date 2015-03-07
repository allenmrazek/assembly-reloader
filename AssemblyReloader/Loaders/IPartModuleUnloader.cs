using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Loaders
{
    public interface IPartModuleUnloader
    {
        void Unload(Type type);
    }
}
