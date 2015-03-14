using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloader.Queries.CecilQueries.IntermediateLanguage;
using Mono.Cecil;
using Mono.Cecil.Cil;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using ReeperCommon.Logging.Implementations;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;

namespace AssemblyReloader.Weaving.Operations
{
    public class InterceptExecutingAssemblyLocationQueries : WeaveOperation
    {
        private readonly ILog _log;
        private readonly IInstructionSetQuery _getCodeBaseCallQuery;
        private readonly IMethodDefinitionQuery _replacementCallQuery;

        public InterceptExecutingAssemblyLocationQueries(
            ILog log,
            IInstructionSetQuery getCodeBaseCallQuery,
            IMethodDefinitionQuery replacementCallQuery)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (getCodeBaseCallQuery == null) throw new ArgumentNullException("getCodeBaseCallQuery");
            if (replacementCallQuery == null) throw new ArgumentNullException("replacementCallQuery");

            _log = log;
            _getCodeBaseCallQuery = getCodeBaseCallQuery;
            _replacementCallQuery = replacementCallQuery;

        }


        public override void Run(AssemblyDefinition definition)
        {
            // do nothing
        }


        public override void OnEachMethod(MethodDefinition methodDefinition)
        {
            var results = _getCodeBaseCallQuery.Get(methodDefinition).ToList();
            if (results.Count == 0) return;

            var processor = methodDefinition.Body.GetILProcessor();
            var replacement = _replacementCallQuery.Get(methodDefinition.DeclaringType).ToList();

            if (!replacement.Any())
                throw new Exception("Failed to locate replacement call");

            if (replacement.Count > 1)
                throw new Exception("Found too many replacement methods");

            results.ForEach(call =>
            {
                _log.Normal("Found codeBaseCall in " + methodDefinition.FullName);

                // replace this call to the one provided by replacementCallQuery
                processor.Replace(call, processor.Create(OpCodes.Call, replacement.Single()));
            });
        }

        //public override void OnEachMethod(MethodDefinition methodDefinition)
        //{
        //    _log.Normal("Intercept.OnEachMethod");

        //    var codeBaseCalls = _getCodeBaseCallQuery.Get(methodDefinition).ToList();
        //    var uri = new Uri(_location.FullPath);

        //    _log.Normal("Found " + codeBaseCalls.Count + " CodeBase calls in " +
        //                methodDefinition.FullName);

        //    var processor = methodDefinition.Body.GetILProcessor();

        //    foreach (Instruction t in codeBaseCalls)
        //    {
        //        //var loadNull = processor.Create(OpCodes.Ldnull);
        //        //var callTestMethod = processor.Create(OpCodes.Call,
        //        //    methodDefinition.Module.Import(this.GetType()
        //        //        .GetMethod("TestMethod", BindingFlags.Public | BindingFlags.Static)));

        //        //var prevInstruction = t.Previous.Previous; // before the call, before pushing value onto stack

        //        //processor.InsertAfter(prevInstruction, loadNull);
        //        //processor.InsertAfter(loadNull, callTestMethod);

        //        //processor.InsertBefore(t, processor.Create(OpCodes.Pop)); // because this is an instance method
        //        //processor.InsertAfter(t, processor.Create(OpCodes.Ldstr, _location.FullPath));
        //        //processor.Remove(t);
        //    }

        //    //_log.Normal("Uri.LocalPath: " + uri.LocalPath);
        //    //_log.Normal("Uri.AbsolutePath: " + uri.AbsolutePath);
        //    //_log.Normal("Uri.AbsoluteUri: " + Uri.UnescapeDataString(uri.AbsoluteUri));

        //    //if (codeBaseCalls.Count > 0)
        //    //{
        //    //    _log.Normal("Declaring type: " + methodDefinition.DeclaringType.ToString());

        //    //    var checkMethod = new MethodDefinition("GetCodeBaseOfAssembly",
        //    //        MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.Static,
        //    //        methodDefinition.Module.TypeSystem.String);

        //    //    methodDefinition.DeclaringType.Methods.Add(checkMethod);

        //    //    _log.Normal("creating parameter");

        //    //    if (checkMethod.Parameters == null) _log.Normal("no parameters!");

        //    //    checkMethod.Parameters.Add(new ParameterDefinition("assembly", ParameterAttributes.None,
        //    //       checkMethod.Module.Import(typeof(Assembly))));

        //    //    if (checkMethod.Body == null)
        //    //        _log.Error("Method has no body");

        //    //    var p = checkMethod.Body.GetILProcessor();

        //    //    if (p == null) _log.Warning("ILProcessor is null");
        //    //    p.Append(processor.Create(OpCodes.Ldarg_0));
        //    //    p.Append(processor.Create(OpCodes.Ldstr, "Return value"));
        //    //    p.Append(processor.Create(OpCodes.Ret));
        //    //}
        //}

        //public static void TestMethod(Assembly arg)
        //{
        //    UnityEngine.Debug.LogWarning("TestMethod injected");
        //    UnityEngine.Debug.Log("Executing assembly: " + Assembly.GetExecutingAssembly().CodeBase);
        //}

        //public override void OnEachMethod(MethodDefinition methodDefinition)
        //{
        //    _log.Normal("Intercept.OnEachMethod");

        //    foreach (var call in _getExecutingAssemblyCallQuery.Get(methodDefinition))
        //    {
        //        _log.Warning("InterceptExecutingLocation: found call: " + call.Operand.ToString());
        //        _log.Warning("next call is: " + ((call.Next != null && call.Next.Operand is MethodReference)
        //            ? call.Next.Operand.ToString()
        //            : "undefined"));

        //        InterceptAssemblyCodeBase(methodDefinition, call);
        //    }
        //}


        //private void InterceptAssemblyCodeBase(MethodDefinition methodDefinition, Instruction getExecutingAssemblyCall)
        //{
        //    if (methodDefinition == null) throw new ArgumentNullException("methodDefinition");
        //    if (getExecutingAssemblyCall == null) throw new ArgumentNullException("getExecutingAssemblyCall");

        //    var codeBaseCalls = _getCodeBaseCallQuery.Get(methodDefinition).ToArray();

        //    if (!codeBaseCalls.Any())
        //    {
        //        _log.Verbose("Didn't find any codeBase calls in " + methodDefinition.FullName);
        //        return;
        //    }


        //    for (int i = 0; i < codeBaseCalls.Length; ++i)
        //    {
        //        var cbCall = codeBaseCalls[i];

        //        _log.Normal("Found CodeBase call");

        //        if (cbCall.Previous == null || !ReferenceEquals(cbCall.Previous, getExecutingAssemblyCall)) continue;

        //        _log.Normal("Intercepting call");

        //        // rip out this instruction
        //        var processor = methodDefinition.Body.GetILProcessor();
        //        processor.Remove(cbCall);

        //        //// add a return for CodeBase, UNC format
        //        // file:///D:/For New Computer/Kerbal Space Program/GameData/DebugTools/DebugTools.dll

        //        processor.InsertAfter(getExecutingAssemblyCall, processor.Create(OpCodes.Ldstr, _location.FullPath));
        //        processor.Remove(getExecutingAssemblyCall);
        //        return;
        //    }
        //}
    }
}
