namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IQueryPartIsPrefabClone
    {
        bool Get(IPart queryPart, IPart prefabPart);
    }
}
