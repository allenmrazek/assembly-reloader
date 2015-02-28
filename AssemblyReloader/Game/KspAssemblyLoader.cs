using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.Game
{
    public class KspAssemblyLoader : IAssemblyLoader
    {
        private readonly ILog _log;

        public KspAssemblyLoader(ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");
            _log = log;
        }

        public Maybe<Assembly> Load(MemoryStream stream, IFile location)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (location == null) throw new ArgumentNullException("location");

            var assembly = AppDomain.CurrentDomain.Load(stream.GetBuffer());

            if (assembly.IsNull()) return Maybe<Assembly>.None;

            AssemblyLoader.loadedAssemblies.Add(new AssemblyLoader.LoadedAssembly(assembly, location.FullPath,
                location.Url, null));

            return Maybe<Assembly>.With(assembly);
        }
    }
}
