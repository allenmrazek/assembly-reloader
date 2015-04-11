namespace AssemblyReloader.Game.Providers
{
// ReSharper disable once InconsistentNaming
    public interface ICurrentStartupSceneProvider
    {
        KSPAddon.Startup Get();
    }
}
