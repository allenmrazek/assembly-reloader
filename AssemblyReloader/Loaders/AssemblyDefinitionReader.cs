using System;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Loaders
{
    public class AssemblyDefinitionReader : IAssemblyDefinitionReader
    {
        private readonly BaseAssemblyResolver _resolver;

        public AssemblyDefinitionReader(IFile location, BaseAssemblyResolver resolver)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (resolver == null) throw new ArgumentNullException("resolver");

            _resolver = resolver;
            Location = location;
        }


        public Maybe<AssemblyDefinition> Get()
        {
            var definition = AssemblyDefinition.ReadAssembly(Location.FullPath, new ReaderParameters
            {
                AssemblyResolver = _resolver,
            });

            return definition.IsNull() ? Maybe<AssemblyDefinition>.None : Maybe<AssemblyDefinition>.With(definition);
        }

        public IFile Location { get; private set; }

        public string Name
        {
            get { return Location.Name; }
        }
    }
}
