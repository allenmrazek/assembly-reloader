using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Generators;
using AssemblyReloader.Providers;
using Mono.Cecil;

namespace AssemblyReloader.CompositeRoot.Commands.ILModifications
{
    public class RenameAssemblyCommand : ICommand<AssemblyDefinition>
    {
        private readonly IUniqueAssemblyNameGenerator _uniqueNameGenerator;

        public RenameAssemblyCommand(IUniqueAssemblyNameGenerator uniqueNameGenerator)
        {
            if (uniqueNameGenerator == null) throw new ArgumentNullException("uniqueNameGenerator");
            _uniqueNameGenerator = uniqueNameGenerator;
        }

        public void Execute(AssemblyDefinition context)
        {
            context.Name.Name = _uniqueNameGenerator.Get(context);
        }
    }
}
