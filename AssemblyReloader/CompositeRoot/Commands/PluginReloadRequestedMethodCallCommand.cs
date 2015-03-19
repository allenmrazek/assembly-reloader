using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Object = UnityEngine.Object;

namespace AssemblyReloader.CompositeRoot.Commands
{
    public class PluginReloadRequestedMethodCallCommand : ICommand<UnityEngine.Object>
    {
        private const string PluginReloadRequestedMethodCallName = "OnPluginReloadRequested";

        public void Execute(Object context)
        {
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
