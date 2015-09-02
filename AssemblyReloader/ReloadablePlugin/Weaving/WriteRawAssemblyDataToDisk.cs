using System;
using System.IO;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using strange.extensions.injector;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public class WriteRawAssemblyDataToDisk : IRawAssemblyDataFactory
    {
        private readonly IRawAssemblyDataFactory _decorated;
        private readonly IWeaverSettings _settings;
        private readonly Func<ReeperAssembly, string> _getFileName;
        private readonly ILog _log;

        public WriteRawAssemblyDataToDisk(
            [Name(Config.RawAssemblyDataKey.Woven)] IRawAssemblyDataFactory decorated, 
            IWeaverSettings settings, 
            Func<ReeperAssembly, string> getFileName,
            ILog log)
        {
            if (decorated == null) throw new ArgumentNullException("decorated");
            if (settings == null) throw new ArgumentNullException("settings");
            if (getFileName == null) throw new ArgumentNullException("getFileName");
            if (log == null) throw new ArgumentNullException("log");
            _decorated = decorated;
            _settings = settings;
            _getFileName = getFileName;
            _log = log;
        }


        public RawAssemblyData Create(ReeperAssembly assembly)
        {
            var raw = _decorated.Create(assembly);

            if (_settings.WritePatchedAssemblyDataToDisk)
                WritePatchedDataToDisk(raw);

            return raw;
        }


        private void WritePatchedDataToDisk(RawAssemblyData raw)
        {
            var dir = Path.GetDirectoryName(raw.ReeperAssembly.File.fullPath) + Path.DirectorySeparatorChar;
            var patchedPath = dir + _getFileName(raw.ReeperAssembly);

            using (
                var fs =
                    new FileStream(patchedPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                _log.Verbose("Writing patched assembly to: " + patchedPath);
                raw.RawAssembly.WriteTo(fs);

                raw.SymbolStore.Do(ms =>
                {
                    using (
                        var ss = new FileStream(patchedPath + ".mdb", FileMode.OpenOrCreate,
                            FileAccess.Write))
                        ms.WriteTo(ss);
                });
            }
        }
    }
}
