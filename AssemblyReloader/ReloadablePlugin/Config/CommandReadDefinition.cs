using System.Linq;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.ReloadablePlugin.Weaving;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
    public class CommandReadDefinition : Command
    {
        [Inject] public IFile AssemblyFile { get; set; }
        [Inject] public IAssemblyDefinitionReader DefinitionReader { get; set; }
        [Inject] public SignalDefinitionWasRead DefinitionWasWasRead { get; set; }

        [Inject(LogKeys.PluginContext)] public ILog Log { get; set; }


        public override void Execute()
        {
            Log.Verbose("Reading assembly definition");

            var result = DefinitionReader.Read(AssemblyFile);

            if (!result.Any())
            {
                Log.Error("Failed to read assembly definition");
                Fail();
            }

            Log.Verbose("Assembly definition read");

            DefinitionWasWasRead.Dispatch(result.Single());
        }
    }
}
