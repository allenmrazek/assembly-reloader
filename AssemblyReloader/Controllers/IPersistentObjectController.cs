using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Controllers
{
    /// <summary>
    /// Controls creation of persistent objects (like PartModules, ScenarioModules, etc) when
    /// a plugin is reloaded. Although the PartLoader and such types are switched correctly,
    /// additional steps might need to be taken. For example, replacing live PartModules with
    /// their updated versions (and ConfigNodes taken from config files or from the type's predecessor)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPersistentObjectController<T>
    {
        void LoadPersistentObjects(Assembly assembly, IFile location);
        void UnloadPersistentObjects(Assembly assembly, IFile location);
    }
}
