namespace AssemblyReloader.Providers
{
// ReSharper disable once InconsistentNaming
    public interface ICurrentKSPStartupSceneProvider
    {
        KSPAddon.Startup Get();
    }
}
