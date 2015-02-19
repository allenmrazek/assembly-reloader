using System;
using System.IO;
using Mono.Cecil;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.ILModifications
{
    public class ModifiedAssemblyFactory : IModifiedAssemblyFactory
    {
        private readonly DefaultAssemblyResolver _resolver;
        private readonly ILog _log;

        public ModifiedAssemblyFactory(DefaultAssemblyResolver resolver, ILog log)
        {
            if (resolver == null) throw new ArgumentNullException("resolver");
            if (log == null) throw new ArgumentNullException("log");

            _resolver = resolver;
            _log = log;
        }


        public IModifiedAssembly Create(IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");


            var definition = AssemblyDefinition.ReadAssembly(location.FullPath, new ReaderParameters
            {
                AssemblyResolver = _resolver,
            });

            if (definition == null) throw new FileLoadException("Could not find " + location.FullPath);

            return new ModifiedAssembly(definition, _log);
        }
    }
}
