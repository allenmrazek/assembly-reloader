extern alias Cecil96;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandChangeDefinitionIdentity : Command
    {
        [Inject] public AssemblyDefinition Definition { get; set; }
        [Inject] public IGetRandomString RandomStringGenerator { get; set; }
        [Inject] public ILog Log { get; set; }


        public override void Execute()
        {
            Log.Debug("Changing plugin identity");

            Definition.Name.Name = RandomStringGenerator.Get();
        }
    }
}
