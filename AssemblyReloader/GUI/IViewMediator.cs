namespace AssemblyReloader.Gui
{
    public interface IViewMediator
    {
        IMainView MainView { set; }
        ISettingsView SettingsView { set; }

        void Reload(IPluginInfo plugin);

        void TogglePluginOptions(IPluginInfo plugin);
        void ToggleOptions();
        void HideMainWindow();
    }
}
