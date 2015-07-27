using System;
using System.IO;
using AssemblyReloader.Properties;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public class GetDebugSymbolsExistForDefinition : IGetDebugSymbolsExistForDefinition
    {
        private const string DebugSymbolExtension = ".mdb";

        public bool Get([NotNull] IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");

            // note: we use the actual file system instead of checking cached files from GameDatabase
            // because GameDatabase will only contain the state of the file at program load; the debug
            // symbol file might come and go during program lifetime though unlikely
            return File.Exists(location.FullPath + DebugSymbolExtension);
        }
    }
}
