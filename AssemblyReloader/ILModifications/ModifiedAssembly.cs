using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace AssemblyReloader.ILModifications
{
    public class ModifiedAssembly : IModifiedAssembly
    {
        private readonly AssemblyDefinition _assemblyDefinition;
        private readonly ILog _log;

        public ModifiedAssembly(AssemblyDefinition assemblyDefinition, ILog log)
        {
            if (assemblyDefinition == null) throw new ArgumentNullException("assemblyDefinition");
            if (log == null) throw new ArgumentNullException("log");

            _assemblyDefinition = assemblyDefinition;
            _log = log;
        }


        public void Rename(Guid newId)
        {
            _assemblyDefinition.Name.Name = newId.ToString() + "." + _assemblyDefinition.Name.Name;
        }


        public void Trampoline(MethodDefinition @from, MethodDefinition to)
        {
            throw new NotImplementedException();
        }


        public MethodDefinition CreateMethod(ModuleDefinition module, TypeDefinition type, string name, MethodAttributes attr)
        {
            if (type == null) throw new ArgumentNullException("type");

            var voidReference = module.Import(typeof(void));

            var method = new MethodDefinition(name, MethodAttributes.Private, voidReference);
            type.Methods.Add(method);

            return method;
        }


        public IEnumerable<MethodDefinition> FindMethod(TypeDefinition type, string name)
        {
            throw new NotImplementedException();
        }


        public void Write(MemoryStream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            _log.Verbose("Writing assembly definition to memory stream");
            _assemblyDefinition.Write(stream);
        }


        public Maybe<Assembly> Load(MemoryStream source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var loaded = Assembly.Load(source.GetBuffer());

            return loaded == null ? Maybe<Assembly>.None : Maybe<Assembly>.With(loaded);
        }
    }
}
