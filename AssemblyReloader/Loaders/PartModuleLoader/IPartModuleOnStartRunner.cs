namespace AssemblyReloader.Loaders.PartModuleLoader
{
    public interface IPartModuleOnStartRunner
    {
        void Add(PartModule target);
        void Clear();
    }
}
