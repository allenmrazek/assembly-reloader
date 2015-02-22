using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using AssemblyReloader.PluginTracking;

namespace AssemblyReloader.Messages
{
    public interface IMessageFactory
    {
        IMessage CreatePluginLoadedMessage(IReloadablePlugin plugin);
        IMessage CreatePluginUnloadedMessage(IReloadablePlugin plugin);
    }
}
