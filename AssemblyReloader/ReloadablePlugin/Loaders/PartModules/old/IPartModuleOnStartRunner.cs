namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules.old
{
    public interface IPartModuleOnStartRunner
    {
        void Add(PartModule target);
        void ClearPartModuleTargets();
    }
}
