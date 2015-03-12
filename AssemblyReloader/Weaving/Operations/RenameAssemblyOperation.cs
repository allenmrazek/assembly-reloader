using System;
using AssemblyReloader.Generators;
using Mono.Cecil;

namespace AssemblyReloader.Weaving.Operations
{
    class RenameAssemblyOperation : WeaveOperation
    {
        private readonly IUniqueAssemblyNameGenerator _nameGenerator;

        public RenameAssemblyOperation(IUniqueAssemblyNameGenerator nameGenerator)
        {
            if (nameGenerator == null) throw new ArgumentNullException("nameGenerator");
            _nameGenerator = nameGenerator;
        }


        public override void Run(AssemblyDefinition definition)
        {
            definition.Name.Name = _nameGenerator.Get(definition);
        }
    }
}
