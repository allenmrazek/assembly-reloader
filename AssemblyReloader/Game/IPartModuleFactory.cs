using System;

namespace AssemblyReloader.Game
{
    public interface IPartModuleFactory
    {
        PartModule AddPseudoModule(Type type, Part part, ConfigNode config, bool forceAwake); // use this for PartModules the game doesn't know about (.reloadables)
        PartModule AddModule(Type type, Part part, ConfigNode config, bool forceAwake); // use this for those it does and thus don't require any special handling
    }
}
