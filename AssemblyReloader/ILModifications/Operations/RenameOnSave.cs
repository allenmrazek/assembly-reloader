using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.ILModifications.Assembly;
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

            var modifiedAssembly = new ModifiedAssembly(_assemblyDefinition);

            foreach (var pmDef in partModuleDefinitions)
            {
                var onSaveMethod = new PartModuleMethodDefinitionQuery(pmDef).GetOnSave();

                if (!onSaveMethod.Any())
                    // we'll have to handle this case at some point; this situation may occur if the author only needs the automatically-persisted KSPFields and has no need for custom method
                {
                    _log.Warning("No custom OnSave found in " + pmDef.FullName);
                    continue;
                    //throw new NotImplementedException("No custom OnSave found in " + pmDef.FullName);
                }
                // create new method
                var newMethod = modifiedAssembly.CreateMethod(_assemblyDefinition.MainModule, pmDef, "AddedByCecil",
                    MethodAttributes.Public);

                newMethod.Parameters.Add(new ParameterDefinition("testParam", ParameterAttributes.In, _assemblyDefinition.MainModule.Import(typeof (string))));

                // edit this method now

            }
        }
    }
}
