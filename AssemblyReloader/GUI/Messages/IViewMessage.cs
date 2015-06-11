using AssemblyReloader.Controllers;

namespace AssemblyReloader.Gui.Messages
{
    public interface IViewMessage
    {
        IReloadablePlugin Plugin { get; }
    }
}
