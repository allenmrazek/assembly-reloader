extern alias KSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using EventReport = KSP::EventReport;
using GameEvents = KSP::GameEvents;
using EventVoid = KSP::EventVoid;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class CommandRewriteGameEventCalls : Command
    {
        private readonly AssemblyDefinition _context;
        private readonly IGetGameEventTypes _gameEvents;
        private readonly ILog _log;

        public CommandRewriteGameEventCalls(
            AssemblyDefinition context, 
            IGetGameEventTypes gameEvents,
            ILog log)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (gameEvents == null) throw new ArgumentNullException("gameEvents");
            if (log == null) throw new ArgumentNullException("log");

            _context = context;
            _gameEvents = gameEvents;
            _log = log;
        }


        public override void Execute()
        {
            _log.Normal("Rewriting GameEvent calls");


            foreach (var method in _context.Modules.SelectMany(module => module.Types).SelectMany(td => td.Methods))
                ReplaceGameEventCallsInMethod(method);

            // todo: IScalarModule.OnMoving?
        }


        private void ReplaceGameEventCallsInMethod(MethodDefinition method)
        {
            for (int i = 0; i < 4; ++i) // up to 3 generic arguments (0 is for EventVoid)
                foreach (var evt in _gameEvents.Get(i))
                {
                    var typeThatOwnsMethodsToIntercept = method.Module.Import(evt);

                    var calls = GetCallsToDeclaredType(method, typeThatOwnsMethodsToIntercept);

                    if (!calls.Any()) continue;

                    var processor = method.Body.GetILProcessor();

                    while (calls.Any())
                    {
                        var next = calls.Pop();
                        var m = next.Operand as MethodReference;

                        if (m == null)
                        {
                            // this shouldn't happen; added to make resharper happy and catch mistakes
                            _log.Error(
                                "Failed to cast instruction operand to MethodReference in ReplaceGameEventCallsInMethod");
                            continue;
                        }

                        _log.Verbose("Replacing " + m.FullName + " call of " + typeThatOwnsMethodsToIntercept.Name + " at " +
                                     next.Offset + " in " + method.FullName);

                        switch (m.Name)
                        {
                            case "Add":
                                processor.Replace(next,
                                    processor.Create(OpCodes.Callvirt, GetReplacementMethod(evt, "Register", method.Module)));
                                break;

                            case "Remove":
                                processor.Replace(next,
                                    processor.Create(OpCodes.Callvirt, GetReplacementMethod(evt, "Unregister", method.Module)));
                                break;

// ReSharper disable once RedundantCaseLabel
                            case "Fire":
                                // no-op
                                break;

                            default:
                                _log.Warning("Unrecognized GameEvent type method called: " + m.FullName);
                                break;
                        }
                    }
                }
        }


        private Stack<Instruction> GetCallsToDeclaredType(MethodDefinition method, TypeReference declaredType)
        {
            var calls = new Stack<Instruction>();

            foreach (var instr in method.Body.Instructions)
            {
                if (instr.OpCode != OpCodes.Callvirt) continue;

                var methodOperand = instr.Operand as MethodReference;

                if (methodOperand == null) continue;

                if (methodOperand.DeclaringType.FullName == declaredType.FullName)
                    calls.Push(instr);
            }

            return calls;
        }

        //private void ReplaceEventDataAddCallsWithParameters(MethodDefinition method, TypeReference gameEventType, MethodReference replacementCall)
        //{
        //    var processor = method.Body.GetILProcessor();

        //    var targetInstructions = new Queue<Instruction>();
        //    foreach (var instr in method.Body.Instructions)
        //    {
        //        if (instr.OpCode == OpCodes.Callvirt)
        //        {
        //            var methodOperand = instr.Operand as MethodReference;

        //            if (methodOperand != null)
        //            {

        //                if (methodOperand.DeclaringType.FullName == gameEventType.FullName &&
        //                    methodOperand.Name == "Add")
        //                    targetInstructions.Enqueue(instr);
        //            }
        //        }
        //    }

        //    while (targetInstructions.Any())
        //    {
        //        var next = targetInstructions.Dequeue();
        //        _log.Normal("Replacing call in " + method.FullName + " at " + next.Offset + " for " + gameEventType.FullName);
        //        processor.Replace(next, processor.Create(OpCodes.Callvirt, replacementCall));
        //    }
        //}



        private static MethodReference GetReplacementMethod(Type gameEventType, string replacementName, ModuleDefinition module)
        {
            if (!gameEventType.IsGenericType)
            {
                var replacement = typeof (GameEventProxy).GetMethod(replacementName,
                    BindingFlags.Static | BindingFlags.Public, null,
                    new[] {typeof (EventVoid), typeof (EventVoid.OnEvent)}, null);

                if (replacement.IsNull())
                    throw new ArgumentException(
                        "Did not find method called \"" + replacementName + "\" in " + typeof(GameEventProxy).Name,
                        "replacementName");

                return module.Import(replacement);
            }
            var genericMethod =
                typeof(GameEventProxy).GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Single(m => m.Name == replacementName && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == gameEventType.GetGenericArguments().Length);

            if (genericMethod.IsNull())
                throw new ArgumentException(
                        "Did not find generic method called \"" + replacementName + "\" in " + typeof(GameEventProxy).Name,
                        "replacementName");
            return module.Import(genericMethod.MakeGenericMethod(gameEventType.GetGenericArguments()));
        }


        private static TypeReference GetMethodDeclaringType(Type gameEventType, MethodReference method)
        {
            if (gameEventType.IsGenericType)
                return method.DeclaringType.Resolve()
                    .MakeGenericInstanceType(
                        gameEventType.GetGenericArguments().Select(ga => method.Module.Import(ga)).ToArray());

            return method.DeclaringType;
        }


    
        //private void OnCrash(EventReport report)
        //{
        //    _log.Normal("OnCrash received!");
        //}


        //private void CreateProxy()
        //{
        //    // Standard, non-nested GameEvents
        //    foreach (var field in typeof(GameEvents).GetFields(BindingFlags.Public | BindingFlags.Static)
        //        .Where(fi => IsTypeOfEventData(fi.FieldType)))
        //    {
        //        //_log.Normal("Event: " + field.Name + ", " + field.FieldType.FullName);
        //    }

        //    //IsTypeOfEventData(typeof (GameEvents).GetField("onEditorLoad", BindingFlags.Public | BindingFlags.Static).FieldType);
        //}


        //private static bool IsTypeOfEventData(Type ty)
        //{
        //    if (!ty.IsGenericType)
        //        return ty == typeof(KSP::EventVoid);

        //    var genericDef = ty.GetGenericTypeDefinition();

        //    return genericDef == typeof(KSP::EventData<>) 
        //        || genericDef == typeof(KSP::EventData<,>) 
        //        || genericDef == typeof(KSP::EventData<,,>) 
        //        || genericDef == typeof(KSP::EventData<,,,>);
        //}

        //private void RewriteCallsInMethod(MethodDefinition method)
        //{
        //    if (!method.HasBody)
        //        return;

        //    if (method.Name != "Execute")
        //        return;

        //    var processor = method.Body.GetILProcessor();

        //    var targetFieldLoad =
        //        method.Module.Import(typeof (KSP::GameEvents).GetField("onCrash",
        //            BindingFlags.Public | BindingFlags.Static));

        //    var t1 = typeof (KSP::EventData<EventReport>).GetMethod("Add",
        //        BindingFlags.Public | BindingFlags.Instance);

      
        //    try
        //    {
        //        var genericArgument = method.Module.Import(typeof(KSP::EventReport));
        //        var genericType = method.Module.Import(typeof(KSP::EventData<>));


        //        var addMethod = genericType.Resolve().Methods.SingleOrDefault(md => md.Name == "Add").ToMaybe();

        //        var genericOnEvent = method.Module.Import(typeof (KSP::EventData<>.OnEvent));

        //        var test = genericOnEvent.MakeGenericInstanceType(genericArgument);
        //        var createdType = genericType.MakeGenericInstanceType(genericArgument);

        //        var createdResolve = createdType.Resolve();
        //        var addMethodOfCreated =
        //            createdResolve.Methods.Single(m => m.Name == "Add").Resolve().MakeHostInstanceGeneric(genericArgument);

        //        var onEventInResolve = createdResolve.NestedTypes.Single(nt => nt.Name == "OnEvent");

        //        var onEventInResolveResolved = onEventInResolve.Resolve();

        //        var search = addMethod.Single().MakeHostInstanceGeneric(test.Resolve());
        //        var s1 = addMethod.Single().MakeHostInstanceGeneric(onEventInResolveResolved).Resolve();;

        //        var onEvent = method.Module.Import(typeof (KSP::EventData<KSP::EventReport>));
        //        var am = onEvent.Resolve().Methods.Single(md => md.Name == "Add");
        //        var s2 = am.MakeHostInstanceGeneric(onEvent.Resolve()).Resolve();

        //        //var addResolved = addMethod.Single().MakeHostInstanceGeneric(new[]
        //        //{
        //        //    method.Module.Import(
        //        //        genericType.MakeGenericInstanceType(genericArgument)
        //        //            .Resolve()
        //        //            .Methods.Single(m => m.Name == "Add"))
        //        //});

        //        //var addFunc = method.Module.Import(typeof(KSP::EventData<>).GetMethod("Add", BindingFlags.Public | BindingFlags.Instance));
        //        //var r2 = method.Module.Import(addFunc).Resolve();

        //        //var host = addFunc.MakeHostInstanceGeneric(test);

        //        //var r1 = genericType.MakeGenericInstanceType(genericArgument);

        //        //var addFuncGeneric = r1.Resolve().Methods.SingleOrDefault(md => md.Name == "Add");

        //        //var host2 = addFuncGeneric.MakeHostInstanceGeneric(genericType);

        //        //var lazyType = method.Module.Import(typeof(KSP::EventData<>)).MakeGenericInstanceType(genericArgument);



        //        //var funcType = method.Module.Import(typeof(Func<>)).MakeGenericInstanceType(genericArgument);
        //        //var funcCtor =
        //        //     method.Module.Import(funcType.Resolve()
        //        //                                    .Methods.First(m => m.IsConstructor && m.Parameters.Count == 2))
        //        //                    .MakeHostInstanceGeneric(genericArgument);

        //        //var lazyType = method.Module.Import(typeof(Action<>)).MakeGenericInstanceType(genericArgument);
        //        //var lazyCtor =
        //        //     method.Module.Import(lazyType.Resolve()
        //        //                                    .GetConstructors()
        //        //                                    .First(m => m.Parameters.Count == 1
        //        //                                             && m.Parameters[0].ParameterType.Name.StartsWith("Func")))
        //        //                    .MakeHostInstanceGeneric(genericArgument);

        //        //var lazyCtor =
        //        //    method.Module.Import(lazyType.Resolve()
        //        //                                    .GetConstructors()
        //        //                                    .First(m => m.Parameters.Count == 1
        //        //                                             && m.Parameters[0].ParameterType.Name.StartsWith("Func")))
        //        //                    .MakeHostInstanceGeneric(genericArgument);

        //        // Method body as above

        //        //var t1Generic =
        //        //    typeof (KSP::EventData<>).GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
        //        //        //.MakeGenericMethod(new[] {typeof (KSP::EventReport)}));
        //        //var t1import = method.Module.Import(t1Generic);
        //        //var t1resolve = method.Module.Import(t1Generic).Resolve();

        //        //var t2 = typeof (KSP::EventData<EventReport>).GetMethod("Add",
        //        //    BindingFlags.Public | BindingFlags.Instance);

        //        //var t2import = method.Module.Import(t2);
        //        //var t2resolve = t2import.Resolve();

        //        //var t1Concrete = t1Generic.MakeGenericMethod(new[] {typeof (KSP::EventReport)});

                

        //        _log.Normal("success");
        //    }
        //    catch (Exception e)
        //    {
        //        _log.Warning("t1generic failed, exception: " + e);
        //    }

        //    var targetMethod =
        //        method.Module.Import(t1);

        //    if (targetMethod == null)
        //        _log.Error("couldn't find target method");

        //    var resolved = targetMethod.Resolve();


        //    //var generic = typeof (GameEventProxy).GetMethod("Register", BindingFlags.Static | BindingFlags.Public);
        //    //if (generic == null)
        //    //    _log.Error("couldn't find register");

        //    //var onCrashType =
        //    //    generic.MakeGenericMethod(new[]
        //    //    {typeof (KSP::EventReport)});
        //    //if (onCrashType == null)
        //    //    _log.Error("Failed to create generic type");

        //    //var redirectTo = method.Module.Import(onCrashType);

        //    foreach (var instruction in method.Body.Instructions)
        //    {
        //        // replace EventData<>.Add calls with GameEventProxy.Register<> calls
        //        if (instruction.OpCode == OpCodes.Callvirt)
        //            if (instruction.Operand == targetMethod)
        //            {
        //                _log.Warning("Found a target method: " + instruction.Operand.GetType().FullName);
        //            }

        //        if (instruction.Operand is GenericInstanceMethod)
        //        {
        //            _log.Warning("operand is generic method");
        //        }

        //        if (instruction.Operand is GenericInstanceType)
        //        {
        //            _log.Warning("operand is generic type");
        //        }
        //    }
        //}
        //private void RewriteCallsInMethod(MethodDefinition method)
        //{
        //    if (!method.HasBody)
        //        return;

        //    var processor = method.Body.GetILProcessor();
        //    var target = method.Module.Import(typeof(GameEvents).GetField("onEditorLoad", BindingFlags.Public | BindingFlags.Static));

        //    // if we find the assembly loading up a static field for a GameEvent,
        //    // 
        //    foreach (var instruction in method.Body.Instructions)
        //    {
        //        if (instruction.OpCode == OpCodes.Ldsfld)
        //        {


        //            if (instruction.Operand is FieldReference)
        //            {
        //                var fr = instruction.Operand as FieldReference;



        //                if (fr.FullName == target.FullName)
        //                    _log.Warning("This fieldReference " + target.FullName + " should be intercepted!");
        //                else continue;

        //                _log.Normal("FieldReference: " + fr.FullName);
        //                _log.Normal("Found Ldsfld in " + method.FullName);
        //                _log.Normal("Its type: " + instruction.Operand.GetType().FullName);
        //            }
        //            //if (instruction.Operand
        //        }
        //    }
        //}


    }


    public static class CecilExtensions
    {
        public static MethodReference MakeHostInstanceGeneric(
                                  this MethodReference self,
                                  params TypeReference[] args)
        {
            var reference = new MethodReference(
                self.Name,
                self.ReturnType,
                self.DeclaringType.MakeGenericInstanceType(args))
            {
                HasThis = self.HasThis,
                ExplicitThis = self.ExplicitThis,
                CallingConvention = self.CallingConvention
            };

            foreach (var parameter in self.Parameters)
            {
                reference.Parameters.Add(new ParameterDefinition(parameter.ParameterType));
            }

            foreach (var genericParam in self.GenericParameters)
            {
                reference.GenericParameters.Add(new GenericParameter(genericParam.Name, reference));
            }

            return reference;
        }
    }
}
