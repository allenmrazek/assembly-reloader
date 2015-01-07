using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyReloader.ILModifications
{
    class ModifyPluginIdentity
    {
        public void Rename(AssemblyDefinition definition, Guid newId)
        {
            if (definition == null) throw new ArgumentNullException("definition");

            definition.Name.Name = newId.ToString() + "." + definition.Name.Name;
        }
    }
}
