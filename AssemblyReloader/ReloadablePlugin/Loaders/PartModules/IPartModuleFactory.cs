namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleFactory
    {
        void Create(IPart part, PartModuleDescriptor descriptor);
    }
}
