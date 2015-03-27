using System;
using System.Reflection;
using ReeperCommon.Logging.Implementations;
using Object = UnityEngine.Object;

namespace AssemblyReloader.Commands
{
    public class PluginReloadRequestedMethodCallCommand : ICommand<Object>
    {
        private const string PluginReloadRequestedMethodCallName = "OnPluginReloadRequested";

        public void Execute(Object context)
        {
            // todo: remove in release
            new DebugLog().Debug("Executing plugin request command");

            // note: we use reflection rather than SendMessage here because SendMessage will fail
            // if the target component is inactive
            var method = context.GetType().GetMethod(PluginReloadRequestedMethodCallName,
                  BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic,
                  null, new Type[] { }, null);

            if (method != null) // not having such a method is valid
                method.Invoke(context, new object[] { });
        }
    }
}
