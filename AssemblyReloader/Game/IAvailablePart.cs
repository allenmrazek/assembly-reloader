namespace AssemblyReloader.Game
{
    public interface IAvailablePart
    {
        string Name { get; }
        IPart PartPrefab { get; }
    }
}
