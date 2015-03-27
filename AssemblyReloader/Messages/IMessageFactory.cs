using System.Runtime.Remoting.Messaging;
using AssemblyReloader.PluginTracking;

namespace AssemblyReloader.Messages
{
    public interface IMessageFactory
    {
        IMessage CreatePluginLoadedMessage(IReloadablePlugin plugin);
        IMessage CreatePluginUnloadedMessage(IReloadablePlugin plugin);
    }
}
