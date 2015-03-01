namespace AssemblyReloader.Game
{
    public interface IKspFactory
    {
        IPart Create(Part part);
        IAvailablePart Create(AvailablePart part);
        IVessel Create(Vessel vessel);
    }
}
