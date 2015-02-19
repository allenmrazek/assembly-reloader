using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Queries.CecilQueries
{
    public class PartModuleMethodQuery : IPartModuleMethodQuery
    {
        public Maybe<MethodDefinition> GetOnSaveDefinition(TypeDefinition pmDefinition)
        {
            return Get("OnSave", pmDefinition);
        }

        public Maybe<MethodInfo> GetOnSaveMethod(Type type)
        {
            return Get("OnSave", type);
        }

        public Maybe<MethodDefinition> GetOnLoadDefinition(TypeDefinition pmDefinition)
        {
            return Get("OnLoad", pmDefinition);
        }

        public Maybe<MethodInfo> GetOnLoadMethod(Type type)
        {
            return Get("OnLoad", type);
        }

        private Maybe<MethodDefinition> Get(string methodName, TypeDefinition pmDefinition)
        {
            var result = pmDefinition.Methods
                .Where(methodDef => methodDef.Name == methodName)
                .Where(methodDef => methodDef.IsVirtual)
                .Where(methodDef => methodDef.Parameters.First().ParameterType.FullName == typeof(ConfigNode).FullName)
                .Where(methodDef => methodDef.Parameters.Count == 1)
                .ToList();

            if (result.Count > 1)
                throw new InvalidOperationException("Multiple methods match " + methodName + " in " + pmDefinition.FullName);

            return result.Count == 0 ? Maybe<MethodDefinition>.None : Maybe<MethodDefinition>.With(result.Single());
        }


        private Maybe<MethodInfo> Get(string methodName, Type type)
        {
            var mi = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly, null, new [] { typeof(ConfigNode)}, null);

            if (mi.IsNull()) return Maybe<MethodInfo>.None;

            return (mi.IsVirtual && mi.IsPublic) ? Maybe<MethodInfo>.With(mi) : Maybe<MethodInfo>.None;
        }
    }
}
