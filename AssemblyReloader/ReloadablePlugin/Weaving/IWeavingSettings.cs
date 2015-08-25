namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public interface IWeavingSettings
    {
        bool InterceptGameEvents { get; }
        bool DontInlineFunctionsThatCallGameEvents { get; }
    }
}
