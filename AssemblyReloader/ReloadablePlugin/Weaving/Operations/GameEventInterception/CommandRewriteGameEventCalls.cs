extern alias KSP;
extern alias Cecil96;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using EventVoid = KSP::EventVoid;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using ModuleDefinition = Cecil96::Mono.Cecil.ModuleDefinition;
using TypeReference = Cecil96::Mono.Cecil.TypeReference;
using MethodReference = Cecil96::Mono.Cecil.MethodReference;
using MethodDefinition = Cecil96::Mono.Cecil.MethodDefinition;
using ILProcessor = Cecil96::Mono.Cecil.Cil.ILProcessor;
using Instruction = Cecil96::Mono.Cecil.Cil.Instruction;
using OpCodes = Cecil96::Mono.Cecil.Cil.OpCodes;


namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandRewriteGameEventCalls : Command
    {
        private readonly AssemblyDefinition _context;
        private readonly IGetGameEventTypes _gameEvents;
        private readonly IWeaverSettings _weaverSettings;
        private readonly ILog _log;

        public CommandRewriteGameEventCalls(
            AssemblyDefinition context, 
            IGetGameEventTypes gameEvents,
            IWeaverSettings weaverSettings,
            ILog log)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (gameEvents == null) throw new ArgumentNullException("gameEvents");
            if (weaverSettings == null) throw new ArgumentNullException("weaverSettings");
            if (log == null) throw new ArgumentNullException("log");

            _context = context;
            _gameEvents = gameEvents;
            _weaverSettings = weaverSettings;
            _log = log;
        }


        public override void Execute()
        {
            if (!_weaverSettings.InterceptGameEvents)
                return;

            _log.Normal("Rewriting GameEvent calls");


            foreach (var method in _context.Modules.SelectMany(module => module.Types).SelectMany(td => td.Methods))
                ReplaceGameEventCallsInMethod(method);

            // todo: IScalarModule.OnMoving?

            _log.Normal("Finished rewriting GameEvent calls");
        }


        private void ReplaceGameEventCallsInMethod(MethodDefinition method)
        {
            for (int i = 0; i < 4; ++i) // up to 3 generic arguments (0 is for EventVoid)
                foreach (var evt in _gameEvents.Get(i))
                {
                    var typeThatOwnsMethodsToIntercept = method.Module.Import(evt);
                    var calls = GetCallsToDeclaredType(method, typeThatOwnsMethodsToIntercept);

                    if (!calls.Any()) continue;

                    RewriteInstructions(method, calls, typeThatOwnsMethodsToIntercept, evt);
                }
        }


        private Stack<Instruction> GetCallsToDeclaredType(MethodDefinition method, TypeReference declaredType)
        {
            var calls = new Stack<Instruction>();

            foreach (var instr in method.Body.Instructions)
            {
                var instruction = instr;
                //if (instr.OpCode != OpCodes.Callvirt) continue;

                //instr.With(i => i.Operand as MethodReference)

                instr
                    .If(i => i.OpCode == OpCodes.Callvirt)
                    .With(i => i.Operand as MethodReference)
                    .If(mr => mr.DeclaringType.FullName == declaredType.FullName)
                    .Do(mr => calls.Push(instruction))
                    .Do(mr => _log.Debug("Added target instruction in " + mr.FullName));

                //var methodOperand = instr.Operand as MethodReference;

                //if (methodOperand == null) continue;

                //if (methodOperand.DeclaringType.FullName == declaredType.FullName)
                //    calls.Push(instr);
            }

            return calls;
        }


        private void RewriteInstructions(MethodDefinition method, Stack<Instruction> instructionsWithTargetedCalls, TypeReference typeThatOwnsInterceptedMethods, Type gameEventType)
        {
            var processor = method.Body.GetILProcessor();

            // this to help make StackTrace in GameEventProxy more informative, else the compiler
            // might optimize small methods at runtime by inlining them and so the caller info
            // is removed or potentially points to the wrong method
            if (!method.NoInlining)
                if (_weaverSettings.DontInlineFunctionsThatCallGameEvents)
                    method.NoInlining = true;

            while (instructionsWithTargetedCalls.Any())
            {
                var next = instructionsWithTargetedCalls.Pop();
                var m = next.Operand as MethodReference;

                if (m == null)
                {
                    // this shouldn't happen; added to make resharper happy and catch mistakes
                    _log.Error(
                        "Failed to cast instruction operand to MethodReference");
                    continue;
                }

                _log.Verbose("Replacing " + m.FullName + " call of " + typeThatOwnsInterceptedMethods.Name + " at " +
                    next.Offset + " in " + method.FullName);
                RewriteInstruction(processor, m, next, gameEventType);
            }
        }


        private void RewriteInstruction(ILProcessor processor, MethodReference method, Instruction instruction, Type gameEventType)
        {
            switch (method.Name)
            {
                case "Add":
                    processor.Replace(instruction,
                        processor.Create(OpCodes.Callvirt, GetReplacementMethod(gameEventType, "Register", method.Module)));
                    break;

                case "Remove":
                    processor.Replace(instruction,
                        processor.Create(OpCodes.Callvirt, GetReplacementMethod(gameEventType, "Unregister", method.Module)));
                    break;

                // ReSharper disable once RedundantCaseLabel
                case "Fire":
                    // no-op
                    break;

                default:
                    _log.Warning("Unrecognized GameEvent type method called: " + method.FullName);
                    break;
            }
        }


        private static MethodReference GetReplacementMethod(Type gameEventType, string replacementName, ModuleDefinition module)
        {
            const BindingFlags flags = BindingFlags.Static | BindingFlags.Public;

            if (!gameEventType.IsGenericType)
            {
                var replacement = typeof (GameEventProxy).GetMethod(replacementName,
                    flags, null,
                    new[] {typeof (EventVoid), typeof (EventVoid.OnEvent)}, null);

                if (replacement.IsNull())
                    throw new ArgumentException(
                        "Did not find method called \"" + replacementName + "\" in " + typeof(GameEventProxy).Name,
                        "replacementName");

                return module.Import(replacement);
            }

            var genericMethod =
                typeof(GameEventProxy).GetMethods(flags)
                    .Single(m => m.Name == replacementName && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == gameEventType.GetGenericArguments().Length);

            if (genericMethod.IsNull())
                throw new ArgumentException(
                        "Did not find generic method called \"" + replacementName + "\" in " + typeof(GameEventProxy).Name,
                        "replacementName");
            return module.Import(genericMethod.MakeGenericMethod(gameEventType.GetGenericArguments()));
        }
    }
}
