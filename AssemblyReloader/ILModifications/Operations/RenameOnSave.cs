using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            //var results = _assemblyDefinition.MainModule.Types;
            //var results = _assemblyDefinition.MainModule.Types.SelectMany(GetAllTypes);
            var results = _assemblyDefinition.MainModule.GetTypes();
            results.ToList().ForEach(result =>
            {
                _log.Normal("Type: " + result.FullName);
                _log.Normal("  BaseType: " + (result.BaseType != null ? result.BaseType.FullName : "<null>"));

                if (IsPartModule(result))
                    _log.Normal("  *** PartModule ***");
            });
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
