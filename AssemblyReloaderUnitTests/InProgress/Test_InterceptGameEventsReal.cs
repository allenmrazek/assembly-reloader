using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception;
using AssemblyReloaderTests.Fixtures;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using WeavingTestData.GameEventsMock;
using Xunit;
using Xunit.Extensions;

namespace AssemblyReloaderTests.InProgress
{

// ReSharper disable once InconsistentNaming
    public class Test_InterceptGameEventsReal
    {
        [Theory, MockGameEventsDomainData]
        void Check(AssemblyDefinition definition, TypeDefinition type)
        {
            var genericType = type.Module.Import(typeof(EventData<>));
            var genericParam = type.Module.Import(typeof(EventReport));
            var methodWithGameEventsAdd =
                definition.MainModule.GetType("WeavingTestData.GameEventsReal.AddGameEvent").Methods.Single(m => m.Name == "Execute");

            var definedOwningType = genericType.MakeGenericInstanceType(genericParam);
            var definedTypeAddMethod = definedOwningType.Resolve().Methods.Single(m => m.Name == "Add");

            var processor = methodWithGameEventsAdd.Body.GetILProcessor();
            methodWithGameEventsAdd.Body.SimplifyMacros();

            var methodGeneric = typeof (TestReplacement).GetMethods(BindingFlags.Public | BindingFlags.Static).Single(mi => mi.IsGenericMethod && mi.Name == "Register" && mi.GetGenericArguments().Length == 1);

            var replacementCall =
                type.Module.Import(methodGeneric.MakeGenericMethod(typeof(EventReport)));

            // look for add calls
            foreach (var i in methodWithGameEventsAdd.Body.Instructions)
            {
                if (i.OpCode == OpCodes.Callvirt)
                {
                    var method = i.Operand as MethodReference;

                    if (method != null)
                        if (method.DeclaringType.FullName == definedOwningType.FullName && method.Name == "Add")
                        {
                            processor.Replace(i, processor.Create(OpCodes.Callvirt, replacementCall));
                            break; // note: this only replaces one due to editing iterator
                        }
                }
            }

            Assert.NotNull(definedTypeAddMethod);
            Assert.NotNull(methodWithGameEventsAdd);
            Assert.NotNull(genericType);
            Assert.NotNull(genericParam);
            Assert.NotNull(definedOwningType);
            Assert.True(
                methodWithGameEventsAdd.Body.Instructions.Any(
                    i => i.OpCode == OpCodes.Callvirt && ((MethodReference)i.Operand).FullName == replacementCall.FullName));
        }


        [Theory, RealGameEventsDomainData]
        public void VariableArgumentInterception(AssemblyDefinition context, GetGameEventTypes gameEvents)
        {
            int replacements = 0;

            foreach (var method in context.Modules.SelectMany(module => module.Types).Where(td => td.Namespace.Contains("GameEventsReal")).SelectMany(td => td.Methods))
                for (int i = 0; i < 4; ++i)
                {
                    foreach (var evt in gameEvents.Get(i))
                    {
                        if (!method.HasBody)
                            break;

                        var targetaddMethod = method.Module.Import(evt).Resolve().Methods.Single(m => m.Name == "Add");
                        var addMethodDeclaringType = GetMethodDeclaringType(evt, targetaddMethod);
                        var registerCall = GetRegisterMethod(evt, method.Module);

                        Assert.NotNull(targetaddMethod);
                        Assert.NotNull(registerCall);
                        Assert.NotNull(addMethodDeclaringType);

                        var processor = method.Body.GetILProcessor();

                        var targetInstructions = new Queue<Instruction>();
                        foreach (var instr in method.Body.Instructions)
                        {
                            if (instr.OpCode == OpCodes.Callvirt)
                            {
                                var methodOperand = instr.Operand as MethodReference;

                                if (methodOperand != null)
                                {

                                    if (methodOperand.DeclaringType.FullName == addMethodDeclaringType.FullName &&
                                        methodOperand.Name == "Add")
                                        targetInstructions.Enqueue(instr);
                                }
                            }
                        }

                        while (targetInstructions.Any())
                        {
                            var next = targetInstructions.Dequeue();

                            ++replacements;
                            processor.Replace(next, processor.Create(OpCodes.Callvirt, registerCall));
                        }
                    }
                }

            Assert.True(replacements > 0);
#if DEBUG
            //context.Write("gameEventsRealResult.dll");
#endif
        }

        [Theory, RealGameEventsDomainData]
        public void VariableArgumentInterception_UsingDeclaringTypeImportDirectly(AssemblyDefinition context, GetGameEventTypes gameEvents)
        {
            int replacements = 0;

            foreach (var method in context.Modules.SelectMany(module => module.Types).Where(td => td.Namespace.Contains("GameEventsReal")).SelectMany(td => td.Methods))
                for (int i = 0; i < 4; ++i)
                {
                    foreach (var evt in gameEvents.Get(i))
                    {
                        if (!method.HasBody)
                            break;

                        TypeReference targetType;

                        var simpleImport = method.Module.Import(evt);

                        if (evt.IsGenericType)
                            targetType =
                                method.Module.Import(evt)
                                    .Resolve()
                                    .MakeGenericInstanceType(
                                        evt.GetGenericArguments().Select(ga => method.Module.Import(ga)).ToArray());
                        else targetType = method.Module.Import(evt).Resolve();

                        var registerCall = GetRegisterMethod(evt, method.Module);


                        var processor = method.Body.GetILProcessor();

                        var targetInstructions = new Queue<Instruction>();
                        foreach (var instr in method.Body.Instructions)
                        {
                            if (instr.OpCode == OpCodes.Callvirt)
                            {
                                var methodOperand = instr.Operand as MethodReference;

                                if (methodOperand != null)
                                {

                                    if (methodOperand.DeclaringType.FullName == targetType.FullName &&
                                        methodOperand.Name == "Add")
                                        targetInstructions.Enqueue(instr);
                                }
                            }
                        }

                        while (targetInstructions.Any())
                        {
                            var next = targetInstructions.Dequeue();

                            ++replacements;
                            processor.Replace(next, processor.Create(OpCodes.Callvirt, registerCall));
                        }
                    }
                }

            Assert.True(replacements > 0);
#if DEBUG
            context.Write("gameEventsRealResult.dll");
#endif
        }

        private static MethodReference GetRegisterMethod(Type gameEventType, ModuleDefinition module)
        {
            if (!gameEventType.IsGenericType)
                return
                    module.Import(typeof (TestReplacement).GetMethod("Register",
                        BindingFlags.Static | BindingFlags.Public, null,
                        new[] {typeof (EventVoid), typeof (EventVoid.OnEvent)}, null));

            var genericMethod =
                typeof (TestReplacement).GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Single(m => m.Name == "Register" && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == gameEventType.GetGenericArguments().Length);

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
    }
    
}
