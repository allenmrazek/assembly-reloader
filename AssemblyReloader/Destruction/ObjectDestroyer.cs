using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Contracts;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.Destruction
{
    public class ObjectDestroyer : IObjectDestroyer
    {
        private readonly ILog _log;
        private const string DestructionMethodName = "OnPluginReloadRequested";


        public ObjectDestroyer(ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");
            _log = log;
        }


        public void Destroy(MonoBehaviour mb)
        {
            _log.Verbose("Destroying MonoBehaviour " + mb.name + " (" + mb.GetType().FullName + ")");

            InformTargetOfDestruction(mb.gameObject);
            UnityEngine.Object.Destroy(mb.gameObject);
        }


        public void Destroy(PartModule pm)
        {
            if (pm == null) throw new ArgumentNullException("pm");

            _log.Verbose("Destroying PartModule " + pm.moduleName + " (" + pm.GetType().FullName + ")");
            InformComponentOfDestruction(pm);

            pm.part.Modules.Remove(pm);
            UnityEngine.Object.Destroy(pm);
        }


        public void Destroy(ScenarioModule sm)
        {
            throw new NotImplementedException();
        }


        public void Destroy(InternalModule im)
        {
            throw new NotImplementedException();
        }

        public void Destroy(Contract contract)
        {
            throw new NotImplementedException();
        }


        private void InformTargetOfDestruction(GameObject go)
        {
            go.GetComponentsInChildren<Component>(true).ToList().ForEach(c => InformComponentOfDestruction(c));
        }


        private void InformComponentOfDestruction(Component component)
        {
            if (component == null) throw new ArgumentNullException("component");

            var method = GetMethod(DestructionMethodName, component.GetType());

            if (method != null) // not having such a method is valid
                method.Invoke(component, new object[] { });
        }


        private static MethodInfo GetMethod(string name, Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return type.GetMethod(name,
                BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic,
                null, new Type[] { }, null);
        }
    }
}
