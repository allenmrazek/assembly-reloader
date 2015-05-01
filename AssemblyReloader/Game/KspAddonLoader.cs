namespace AssemblyReloader.Game
{
    public class KspAddonLoader : IGameAddonLoader
    {
        public void StartAddons(KSPAddon.Startup scene)
        {
            AddonLoader.Instance.StartAddons(scene);
        }
    }
}
