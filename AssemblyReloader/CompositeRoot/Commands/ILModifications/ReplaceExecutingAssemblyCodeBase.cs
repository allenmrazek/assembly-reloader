﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging.Implementations;

namespace AssemblyReloader.CompositeRoot.Commands.ILModifications
{
    public class ReplaceExecutingAssemblyCodeBase : ICommand<AssemblyDefinition>
    {
        private readonly IFile _location;

        /*
         * [Log]: Location = D:\For New Computer\Kerbal Space Program\GameData\DebugTools\DebugTools.dll
         * [Log]: CodeBase = file:///D:/For New Computer/Kerbal Space Program/GameData/DebugTools/DebugTools.dll
         * */

        public ReplaceExecutingAssemblyCodeBase(IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");
            _location = location;
        }


        public void Execute(AssemblyDefinition context)
        {
            foreach (var m in context.Modules)
                InspectModule(m);
        }


        private void InspectModule(ModuleDefinition moduleDefinition)
        {
            
            foreach (var ty in moduleDefinition.GetTypes())
                InspectType(moduleDefinition, ty);
        }


        private void InspectType(ModuleDefinition moduleDefinition, TypeDefinition typeDefinition)
        {
            
            foreach (var method in typeDefinition.Methods)
                InspectMethod(moduleDefinition, method);
        }


        private void InspectMethod(ModuleDefinition moduleDefinition, MethodDefinition methodDefinition)
        {
            var processor = methodDefinition.Body.GetILProcessor();

            var log = new DebugLog(methodDefinition.FullName);
            Instruction target = null;

            foreach (var inst in processor.Body.Instructions)
            {
                if (inst.OpCode == OpCodes.Call)
                {
                    log.Debug("Operand: " + inst.Operand);

                    log.Debug("Operand Type: " + inst.Operand.GetType().FullName);
                    
                    if ((moduleDefinition.Import(typeof(Assembly).GetMethod("GetExecutingAssembly", BindingFlags.Public | BindingFlags.Static))
                        ).FullName == (inst.Operand as MethodReference).FullName)
                        if (inst.Next.OpCode == OpCodes.Callvirt)
                            if (inst.Next.Operand is MethodReference)
                            {
                                log.Warning("   TARGET MATCH!" + ((MethodReference) inst.Next.Operand).FullName);

                                target = inst;
                                break;
                            }

                }

  
            }

            if (target != null)
            {
                var prev = target.Previous;

                log.Normal("removing previous entries");

                processor.Remove(target.Next); // remove callvirt to get_CodeBase
                processor.Remove(target);

                log.Normal("creating new entry");
                var newInstr = processor.Create(OpCodes.Ldstr, _location.FullPath);

                log.Normal("weaving");
                if (prev != null)
                    processor.InsertAfter(prev, newInstr);
                else processor.Append(newInstr);

                log.Normal("done");
            }
        }
    }
}
