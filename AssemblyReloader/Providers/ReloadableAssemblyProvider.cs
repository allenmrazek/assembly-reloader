using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Providers
{
    public class ReloadableAssemblyProvider : IReloadableAssemblyProvider
    {
        private readonly IFile _location;
        private readonly DefaultAssemblyResolver _resolver;

        public ReloadableAssemblyProvider(IFile location, DefaultAssemblyResolver resolver)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (resolver == null) throw new ArgumentNullException("resolver");
            _location = location;
            _resolver = resolver;
        }


        public Assembly Get()
        {
            using (var stream = new System.IO.MemoryStream())
            {
                var assemblyDefinition = LoadAssemblyDefinitionFromDisk();

                //original.Rename(Guid.NewGuid());

                //original.Write(stream);

                var result = LoadAssemblyFromStream(stream);

                if (result == null)
                    throw new InvalidOperationException("Failed to read assembly from byte stream");

                return result;
            }
        }


        private AssemblyDefinition LoadAssemblyDefinitionFromDisk()
        {
            var definition = AssemblyDefinition.ReadAssembly(_location.FullPath, new ReaderParameters
            {
                AssemblyResolver = _resolver,
            });

            if (definition == null) throw new FileLoadException("Could not find " + _location.FullPath);

            return definition;
        }


        private Assembly LoadAssemblyFromStream(MemoryStream stream)
        {
            return Assembly.Load(stream.GetBuffer());
        }

        public string Name
        {
            get { return _location.Name; }
        }
    }
}
