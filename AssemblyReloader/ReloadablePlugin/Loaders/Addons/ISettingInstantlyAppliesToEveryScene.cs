namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IAddonSettings
    {
        bool InstantlyAppliesToAllScenes { get; }
        bool StartAddonsForCurrentScene { get; }
    }
}
