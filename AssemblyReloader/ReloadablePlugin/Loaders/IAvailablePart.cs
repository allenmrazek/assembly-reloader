namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IAvailablePart
    {
        string Name { get; }
        IPart PartPrefab { get; }
    }
}
