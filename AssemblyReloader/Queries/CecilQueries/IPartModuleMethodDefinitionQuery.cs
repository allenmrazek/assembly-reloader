using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using ReeperCommon.Containers;

namespace AssemblyReloader.Queries.CecilQueries
{
    public interface IPartModuleMethodDefinitionQuery
    {
        Maybe<MethodDefinition> GetOnSave();
        Maybe<MethodDefinition> GetOnLoad();
    }
}
