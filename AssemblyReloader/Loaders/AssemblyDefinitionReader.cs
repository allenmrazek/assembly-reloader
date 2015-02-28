using System;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Loaders
{
    public class AssemblyDefinitionReader : IAssemblyDefinitionReader
    {
        private readonly IFile _location;
        private readonly BaseAssemblyResolver _resolver;

        public AssemblyDefinitionReader(IFile location, BaseAssemblyResolver resolver)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (resolver == null) throw new ArgumentNullException("resolver");

            _location = location;
            _resolver = resolver;
        }


        public Maybe<AssemblyDefinition> Get()
        {
            var definition = AssemblyDefinition.ReadAssembly(_location.FullPath, new ReaderParameters
            {
                AssemblyResolver = _resolver,
            });

            return definition.IsNull() ? Maybe<AssemblyDefinition>.None : Maybe<AssemblyDefinition>.With(definition);
        }

        public string Name
        {
            get { return _location.Name; }
        }
    }
}
