namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleSettings
    {
        bool SaveAndReloadPartModuleConfigNodes { get; }
        bool ReplacePartModulesInstancesImmediately { get; }
    }
}
