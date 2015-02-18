using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using ReeperCommon.Containers;

namespace AssemblyReloader.Queries.CecilQueries
{
    public class PartModuleMethodDefinitionQuery : IPartModuleMethodDefinitionQuery
    {
        private readonly TypeDefinition _pmDefinition;

        public PartModuleMethodDefinitionQuery(TypeDefinition pmDefinition)
        {
            if (pmDefinition == null) throw new ArgumentNullException("pmDefinition");
            _pmDefinition = pmDefinition;
        }


        public Maybe<MethodDefinition> GetOnSave()
        {
            return Get("OnSave");
        }

        public Maybe<MethodDefinition> GetOnLoad()
        {
            return Get("OnLoad");
        }

        private Maybe<MethodDefinition> Get(string methodName)
        {
            var result = _pmDefinition.Methods
                .Where(methodDef => methodDef.Name == methodName)
                .Where(methodDef => methodDef.IsVirtual)
                .Where(methodDef => methodDef.Parameters.First().ParameterType.FullName == typeof(ConfigNode).FullName)
                .Where(methodDef => methodDef.Parameters.Count == 1)
                .ToList();

            if (result.Count > 1)
                throw new InvalidOperationException("Multiple methods match " + methodName + " in " + _pmDefinition.FullName);

            return result.Count == 0 ? Maybe<MethodDefinition>.None : Maybe<MethodDefinition>.With(result.Single());
        }
    }
}
