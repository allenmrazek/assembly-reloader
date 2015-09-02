namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public interface IWeaverSettings
    {
        bool InterceptGameEvents { get; }
        bool DontInlineFunctionsThatCallGameEvents { get; }
        bool WritePatchedAssemblyDataToDisk { get; }
    }
}
