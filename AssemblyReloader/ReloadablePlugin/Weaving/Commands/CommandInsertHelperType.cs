using System.Linq;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using Mono.Cecil;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Commands
{
    public class CommandInsertHelperType : Command
    {
        [Inject(LogKeys.PluginContext)] public ILog Log { get; set; }

        [Inject] public HelperTypeParameters HelperTypeParameters { get; set; }

        [Inject] public AssemblyDefinition AssemblyDefinition { get; set; }


        public override void Execute()
        {
            Log.Debug("Inserting helper type");

            // the main thing that might go wrong is that the type was already inserted (might happen
            // if the user was careless and renamed a dumped version of the assembly)
            if (AssemblyDefinition.Modules.SelectMany(module => module.Types)
                .Any(td => td.Namespace == HelperTypeParameters.Namespace && td.Name == HelperTypeParameters.TypeName))
            {
                Log.Error("Definition already contains injected helper type!");
                Fail();
                return;
            }

            var helperTypeDefinition = new TypeDefinition(
                HelperTypeParameters.Namespace,
                HelperTypeParameters.TypeName,
                TypeAttributes.Class | TypeAttributes.BeforeFieldInit,
                AssemblyDefinition.MainModule.Import(typeof(System.Object)));

            AssemblyDefinition.MainModule.Types.Add(helperTypeDefinition);

            Log.Debug("Helper type successfully inserted");
        }
    }
}
