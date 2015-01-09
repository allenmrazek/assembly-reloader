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
        private readonly AddonsFromAssemblyQuery _assemblyQuery;

        public ReloadableAssemblyFactory(AddonsFromAssemblyQuery assemblyQuery)
        {
            if (assemblyQuery == null) throw new ArgumentNullException("assemblyQuery");
            _assemblyQuery = assemblyQuery;
        }


        public ReloadableAssembly Create(IFile file, LoaderProvider loaderProvider, AddonInfoFactory infoFactory, Log log)
        {
            if (file == null) throw new ArgumentNullException("file");
            if (loaderProvider == null) throw new ArgumentNullException("loaderProvider");
            if (infoFactory == null) throw new ArgumentNullException("infoFactory");
            if (log == null) throw new ArgumentNullException("log");

            return new ReloadableAssembly(file, loaderProvider, infoFactory, log, _assemblyQuery);
        }
    }
}
