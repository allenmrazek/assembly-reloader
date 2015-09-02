extern alias Cecil96;
using System;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using strange.extensions.injector;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using TypeDefinition = Cecil96::Mono.Cecil.TypeDefinition;
using TypeAttributes = Cecil96::Mono.Cecil.TypeAttributes;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class CommandInsertHelperType : Command
    {
        public const string @Namespace = "AssemblyReloaderInjected";
        public const string TypeName = "Helper";

        private readonly SignalHelperDefinitionCreated _helperCreatedSignal;
        private readonly ILog _log;


        [Inject] public AssemblyDefinition Context { get; set; }


        public CommandInsertHelperType(
            SignalHelperDefinitionCreated helperCreatedSignal,
            ILog log)
        {
            if (helperCreatedSignal == null) throw new ArgumentNullException("helperCreatedSignal");
            if (log == null) throw new ArgumentNullException("log");

            _helperCreatedSignal = helperCreatedSignal;
            _log = log;
        }


        public override void Execute()
        {
            var helperDefinition = CreateHelperDefinition();

            if (!helperDefinition.Any())
            {
                _log.Error("Failed to create helper type definition!");
                Fail();
                return;
            }
            _log.Debug("Helper type successfully inserted");

            _helperCreatedSignal.Dispatch(Context, helperDefinition.Single());
        }


        private Maybe<TypeDefinition> CreateHelperDefinition()
        {
            // the main thing that might go wrong is that the type was already inserted (might happen
            // if the user was careless and renamed a dumped version of the assembly)
            if (Context.Modules.SelectMany(module => module.Types)
                .Any(td => td.Namespace == @Namespace && td.Name == TypeName))
            {
                _log.Error("Definition already contains injected helper type!");
                Fail();
                return Maybe<TypeDefinition>.None;
            }

            var helperTypeDefinition = new TypeDefinition(
               @Namespace,
               TypeName,
               TypeAttributes.Class | TypeAttributes.BeforeFieldInit,
               Context.MainModule.Import(typeof(Object)));

            Context.MainModule.Types.Add(helperTypeDefinition);

            return Maybe<TypeDefinition>.With(helperTypeDefinition);
        }
    }
}
