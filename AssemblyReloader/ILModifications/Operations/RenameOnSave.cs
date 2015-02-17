using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Queries.CecilQueries;
using Mono.Cecil;
using ReeperCommon.Logging;

namespace AssemblyReloader.ILModifications.Operations
{
    public class RenameOnSave
    {
        private readonly AssemblyDefinition _assemblyDefinition;
        private readonly ILog _log;

        public RenameOnSave(AssemblyDefinition assemblyDefinition, ILog log)
        {
            if (assemblyDefinition == null) throw new ArgumentNullException("assemblyDefinition");
            if (log == null) throw new ArgumentNullException("log");
            _assemblyDefinition = assemblyDefinition;
            _log = log;
        }

        public void RenameOnSaveMethods(string newName)
        {
            var partModuleDefinitionsQuery = new PartModuleDefinitionsQuery();

            var partModuleDefinitions = partModuleDefinitionsQuery.Get(_assemblyDefinition).ToList();

            partModuleDefinitions.ForEach(d => _log.Normal("PartModule: " + d.FullName));

            foreach (var pmDef in partModuleDefinitions)
            {
                
            }
        }



        //private IEnumerable<TypeDefinition> GetPartModuleDefinitions(ModuleDefinition @in)
        //{
        //    return @in.Types.Where(definition => definition.IsClass)
        //        .Where(definition => definition.BaseType
        //}

        private bool IsPartModule(TypeDefinition type)
        {
            
            return type.Name == "PartModule" && type.Namespace == "UnityEngine";
        }
    }
}
