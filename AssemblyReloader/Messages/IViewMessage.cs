using AssemblyReloader.Controllers;

namespace AssemblyReloader.Messages
{
    public interface IViewMessage
    {
        IReloadablePlugin Plugin { get; }
    }
}
