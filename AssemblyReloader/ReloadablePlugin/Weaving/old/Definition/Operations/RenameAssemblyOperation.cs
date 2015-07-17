using System;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old.Definition.Operations
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
