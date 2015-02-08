namespace AssemblyReloader.Providers.SceneProviders
{
// ReSharper disable once InconsistentNaming
    public interface ICurrentStartupSceneProvider
    {
        KSPAddon.Startup Get();
    }
}
