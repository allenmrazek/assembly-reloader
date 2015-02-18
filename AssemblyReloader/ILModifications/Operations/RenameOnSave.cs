using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.ILModifications.Assembly;
using AssemblyReloader.Loaders.PMLoader;
using AssemblyReloader.Queries.CecilQueries;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using ReeperCommon.Logging;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;

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

                var proxy = _assemblyDefinition.MainModule.Import(typeof (PartModuleProxy));
                

                // create new method
                var newMethod = modifiedAssembly.CreateMethod(_assemblyDefinition.MainModule, pmDef, "AddedByCecil",
                    MethodAttributes.Public | MethodAttributes.FamANDAssem);

                newMethod.Parameters.Add(new ParameterDefinition("testParam", ParameterAttributes.None, _assemblyDefinition.MainModule.Import(typeof (string))));

                var field = new FieldDefinition("Proxy", FieldAttributes.Public, proxy);

                pmDef.Fields.Add(field);

                // edit this method now
                // let's try calling a method on PartModuleProxy
                
                var processor = newMethod.Body.GetILProcessor();

                //var loadArg = processor.Create(OpCodes.Ldloc, field);
                //var loadArg = processor.Create(OpCodes., (ParameterDefinition)null);

                //var methodCall = processor.Create(OpCodes.Call, proxy);

                //processor.Append(loadArg);
                //processor.Append(methodCall);

                _log.Normal("looking for proxy onsave");
                var proxyOnSave = new PartModuleMethodDefinitionQuery(_assemblyDefinition.MainModule.Import(typeof(PartModuleProxy)).Resolve()).GetOnSave();
                _log.Normal("Found proxy? " + (proxyOnSave.Any() ? "yes" : "no"));

                //proxy.Resolve().Methods.ToList().ForEach(md => _log.Normal("Method: " + md.FullName));
                _assemblyDefinition.MainModule.Import(proxyOnSave.Single().Resolve());

                processor.Append(processor.Create(OpCodes.Ldarg_0));
                processor.Append(processor.Create(OpCodes.Ldfld, field.Resolve()));
                processor.Append(processor.Create(OpCodes.Ldarg_0));
                //processor.Append(processor.Create(OpCodes.Call, proxyOnSave.Single().Resolve()));
                processor.Append(processor.Create(OpCodes.Callvirt,
                    _assemblyDefinition.MainModule.Import(typeof (PartModuleProxy).GetMethod("OnSave", BindingFlags.Instance | BindingFlags.Public))));

                //processor.Append(processor.Create(OpCodes.Call, newMethod));
                processor.Append(processor.Create(OpCodes.Ret));
                
            }
        }
    }
}
