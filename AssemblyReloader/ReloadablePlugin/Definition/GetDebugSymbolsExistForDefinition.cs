using System;
using System.IO;
using AssemblyReloader.Properties;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    [Implements(typeof(IGetDebugSymbolsExistForDefinition))]
    public class GetDebugSymbolsExistForDefinition : IGetDebugSymbolsExistForDefinition
    {
        private const string DebugSymbolExtension = ".mdb";

        public bool Get([NotNull] IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");

            // note: we use the actual file system instead of checking cached files from GameDatabase
            // because GameDatabase will only contain the state of the file at program load; the debug
            // symbol file might come and go during art's lifetime
 
            return File.Exists(location.FileName + DebugSymbolExtension);
        }
    }
}
