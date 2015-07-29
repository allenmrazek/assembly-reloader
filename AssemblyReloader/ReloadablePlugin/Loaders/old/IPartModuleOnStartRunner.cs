namespace AssemblyReloader.ReloadablePlugin.Loaders.old
{
    public interface IPartModuleOnStartRunner
    {
        void Add(PartModule target);
        void ClearPartModuleTargets();
    }
}
