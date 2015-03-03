//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using AssemblyReloader.Loaders.PMLoader;
//using AssemblyReloader.Queries.CecilQueries;
//using Mono.Cecil;
//using Mono.Cecil.Cil;
//using Mono.Cecil.Rocks;
//using Mono.CompilerServices.SymbolWriter;
//using ReeperCommon.Logging;
//using FieldAttributes = Mono.Cecil.FieldAttributes;
//using MethodAttributes = Mono.Cecil.MethodAttributes;
//using ParameterAttributes = Mono.Cecil.ParameterAttributes;

//namespace AssemblyReloader.ILModifications.Operations
//{
//    public class RenameOnSave
//    {
//        private readonly AssemblyDefinition _assemblyDefinition;
//        private readonly ILog _log;

//        public RenameOnSave(AssemblyDefinition assemblyDefinition, ILog log)
//        {
//            if (assemblyDefinition == null) throw new ArgumentNullException("assemblyDefinition");
//            if (log == null) throw new ArgumentNullException("log");
//            _assemblyDefinition = assemblyDefinition;
//            _log = log;
//        }


//        public void RenameOnSaveMethods(string newName)
//        {
//            var partModuleDefinitionsQuery = new PartModuleDefinitionsQuery();

//            var partModuleDefinitions = partModuleDefinitionsQuery.Get(_assemblyDefinition).ToList();

//            partModuleDefinitions.ForEach(d => _log.Normal("PartModule: " + d.FullName));

//            var modifiedAssembly = new ModifiedAssembly(_assemblyDefinition);

//            foreach (var pmDef in partModuleDefinitions)
//            {
//                var methodQuery = new PartModuleMethodDefinitionQuery();
//                var onSaveMethod = methodQuery.GetOnSaveDefinition(pmDef);

//                if (!onSaveMethod.Any())
//                    // we'll have to handle this case at some point; this situation may occur if the author only needs the automatically-persisted KSPFields and has no need for custom method
//                {
//                    _log.Warning("No custom OnSave found in " + pmDef.FullName);
//                    continue;
//                    //throw new NotImplementedException("No custom OnSave found in " + pmDef.FullName);
//                }

//                var proxy = _assemblyDefinition.MainModule.Import(typeof (PartModuleProxy));
                

//                // create new method
//                var newMethod = modifiedAssembly.CreateMethod(_assemblyDefinition.MainModule, pmDef, "AddedByCecil",
//                    MethodAttributes.Public | MethodAttributes.FamANDAssem | MethodAttributes.HideBySig);

//                newMethod.Parameters.Add(new ParameterDefinition("testParam", ParameterAttributes.None, _assemblyDefinition.MainModule.Import(typeof (string))));

//                var field = new FieldDefinition("Proxy", FieldAttributes.Public, proxy);

//                pmDef.Fields.Add(field);

//                // edit this method now
//                // let's try calling a method on PartModuleProxy
//                var processor = newMethod.Body.GetILProcessor();

//                processor.Emit(OpCodes.Ldarg_0);
//                processor.Emit(OpCodes.Ldfld, field.Resolve());
//                processor.Emit(OpCodes.Ldarg_0);
//                processor.Emit(OpCodes.Callvirt, _assemblyDefinition.MainModule.Import(methodQuery.GetOnSaveMethod(typeof(PartModuleProxy)).Single()));
//                processor.Emit(OpCodes.Ret);

//            }
//        }
//    }
//}
