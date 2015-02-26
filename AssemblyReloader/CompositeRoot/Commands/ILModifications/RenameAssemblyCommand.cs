using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Providers;
using Mono.Cecil;

namespace AssemblyReloader.CompositeRoot.Commands.ILModifications
{
    public class RenameAssemblyCommand : ICommand<AssemblyDefinition>
    {
        private readonly IUniqueAssemblyNameProvider _uniqueNameProvider;

        public RenameAssemblyCommand(IUniqueAssemblyNameProvider uniqueNameProvider)
        {
            if (uniqueNameProvider == null) throw new ArgumentNullException("uniqueNameProvider");
            _uniqueNameProvider = uniqueNameProvider;
        }

        public void Execute(AssemblyDefinition context)
        {
            context.Name.Name = _uniqueNameProvider.Get(context);
        }
    }
}
