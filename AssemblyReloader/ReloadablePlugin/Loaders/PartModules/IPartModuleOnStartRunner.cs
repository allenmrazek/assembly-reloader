namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleOnStartRunner
    {
        void Add(PartModule target);
        void ClearPartModuleTargets();
    }
}
