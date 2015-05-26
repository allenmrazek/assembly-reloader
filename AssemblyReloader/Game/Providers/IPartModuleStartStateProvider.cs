namespace AssemblyReloader.Game.Providers
{
    public interface IPartModuleStartStateProvider
    {
        PartModule.StartState Get(IVessel vessel);
    }
}
