using System.Runtime.Remoting.Messaging;
using AssemblyReloader.Controllers;

namespace AssemblyReloader.CompositeRoot
{
    public interface IMessageFactory
    {
        IMessage CreatePluginLoadedMessage(IReloadablePlugin plugin);
        IMessage CreatePluginUnloadedMessage(IReloadablePlugin plugin);
    }
}
