using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Queries;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.Factory
{
    class ReloadableAssemblyFactory
    {
        private readonly KspAddonsFromAssemblyQuery _assemblyQuery;

        public ReloadableAssemblyFactory(KspAddonsFromAssemblyQuery assemblyQuery)
        {
            if (assemblyQuery == null) throw new ArgumentNullException("assemblyQuery");
            _assemblyQuery = assemblyQuery;
        }


        public ReloadableAssembly Create(IFile file, LoaderFactory loaderFactory, AddonInfoFactory infoFactory, Log log)
        {
            if (file == null) throw new ArgumentNullException("file");
            if (loaderFactory == null) throw new ArgumentNullException("loaderFactory");
            if (infoFactory == null) throw new ArgumentNullException("infoFactory");
            if (log == null) throw new ArgumentNullException("log");

            return new ReloadableAssembly(file, loaderFactory, infoFactory, log, _assemblyQuery);
        }
    }
}
