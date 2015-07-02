namespace AssemblyReloader.Gui
{
    public interface IMainViewMediator
    {
        IMainView View { get; set; }

        void Reload(IPluginInfo plugin);
        void ToggleOptions(IPluginInfo plugin);
    }
}
