using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    // This attribute will replace KSPAddon in reloadable plugins. This is done so that we can still
    // stick reloadable plugins in the loaded assembly list KSP uses while at the same time cloaking
    // KSPAddons inside said assembly from KSP's AddonLoader, allowing us to implement our own
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ReloadableAddonAttribute : Attribute
    {
        public bool once;
        public KSPAddon.Startup startup;

        public ReloadableAddonAttribute(KSPAddon.Startup startup, bool once)
        {
            this.once = once;
            this.startup = startup;
        }

        public enum Startup
        {
            Instantly = -2,
            EveryScene = -1,
            MainMenu = 2,
            Settings = 3,
            Credits = 4,
            SpaceCentre = 5,
            EditorSPH = 6,
            EditorVAB = 6,
            EditorAny = 6,
            Flight = 7,
            TrackingStation = 8,
            PSystemSpawn = 9,


            None = 99,
        }
    }
}
