namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleSettings
    {
        bool SaveAndReloadPartModuleConfigNodes { get; }
        bool ReloadPartModuleInstancesImmediately { get; }
        bool ResetPartModuleEvents { get; }
        bool ResetPartModuleActions { get; }
    }
}
