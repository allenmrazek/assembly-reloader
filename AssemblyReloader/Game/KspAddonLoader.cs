using AssemblyReloader.StrangeIoC.extensions.implicitBind;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGameAddonLoader))]
// ReSharper disable once UnusedMember.Global
    public class KspAddonLoader : IGameAddonLoader
    {
        public void StartAddons(KSPAddon.Startup scene)
        {
            AddonLoader.Instance.StartAddons(scene);
        }
    }
}
