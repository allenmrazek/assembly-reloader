using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Controllers
{
    public interface IReloadableObjectController
    {
        void Load(Assembly assembly, IFile location);
        void Unload(Assembly assembly, IFile location);
    }
}
