namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IAddonSettings
    {
        bool InstantAppliesToEveryScene { get; }
        bool CreateAddonsForCurrentSceneOnReload { get; }
    }
}
