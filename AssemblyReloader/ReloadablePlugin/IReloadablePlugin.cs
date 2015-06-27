namespace AssemblyReloader.ReloadablePlugin
{
    //public delegate void PluginLoadedHandler(Assembly assembly, IFile location);
    //public delegate void PluginUnloadedHandler(Assembly assembly, IFile location);

    public interface IReloadablePlugin
    {
        bool Reload();

        //event PluginLoadedHandler OnLoaded;
        //event PluginUnloadedHandler OnUnloaded;
    }
}
