using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using ReeperCommon.Containers;

namespace AssemblyReloader.Queries.CecilQueries
{
    public interface IPartModuleMethodQuery
    {
        Maybe<MethodDefinition> GetOnSaveDefinition(TypeDefinition definition);
        Maybe<MethodInfo> GetOnSaveMethod(Type type);

        Maybe<MethodDefinition> GetOnLoadDefinition(TypeDefinition definition);
        Maybe<MethodInfo> GetOnLoadMethod(Type type);
    }
}
