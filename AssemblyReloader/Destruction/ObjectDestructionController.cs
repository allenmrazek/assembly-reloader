using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Contracts;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.Destruction
{
    public class ObjectDestructionController : IObjectDestructionController
    {
        private readonly Dictionary<Type, DestructionHandler> _typeHandlers = new Dictionary<Type, DestructionHandler>(); 

        public ObjectDestructionController()
        {
 
        }


        public void Register<T>(DestructionHandler handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");

            DestructionHandler existingHandler;

            if (_typeHandlers.TryGetValue(typeof (T), out existingHandler))
                _typeHandlers[typeof (T)] = (DestructionHandler) Delegate.Combine(existingHandler, handler);
            else _typeHandlers[typeof (T)] = handler;
        }


        public void Destroy<T>(T target) where T: Component
        {
            if (target == null) throw new ArgumentNullException("target");

            DestructionHandler handler;

            if (!_typeHandlers.TryGetValue(typeof (T), out handler))
                throw new MissingMethodException("Failed to find a destruction handler for " + typeof (T).FullName);

            handler(target, target.gameObject);
        }



        //public void Destroy(MonoBehaviour mb)
        //{
        //    _log.Verbose("Destroying MonoBehaviour " + mb.name + " (" + mb.GetType().FullName + ")");

        //    InformTargetOfDestruction(mb.gameObject);
        //    UnityEngine.Object.Destroy(mb.gameObject);
        //}


        //public void Destroy(PartModule pm)
        //{
        //    if (pm == null) throw new ArgumentNullException("pm");

        //    _log.Verbose("Destroying PartModule " + pm.moduleName + " (" + pm.GetType().FullName + ")");
        //    InformComponentOfDestruction(pm);

        //    pm.part.Modules.Remove(pm);
        //    UnityEngine.Object.Destroy(pm);
        //}


        //public void Destroy(ScenarioModule sm)
        //{
        //    throw new NotImplementedException();
        //}


        //public void Destroy(InternalModule im)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Destroy(Contract contract)
        //{
        //    throw new NotImplementedException();
        //}


        //private void InformTargetOfDestruction(GameObject go)
        //{
        //    go.GetComponentsInChildren<Component>(true).ToList().ForEach(c => InformComponentOfDestruction(c));
        //}


        //private void InformComponentOfDestruction(Component component)
        //{
        //    if (component == null) throw new ArgumentNullException("component");

        //    var method = GetMethod(DestructionMethodName, component.GetType());

        //    if (method != null) // not having such a method is valid
        //        method.Invoke(component, new object[] { });
        //}


        //private static MethodInfo GetMethod(string name, Type type)
        //{
        //    if (type == null) throw new ArgumentNullException("type");

        //    return type.GetMethod(name,
        //        BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic,
        //        null, new Type[] { }, null);
        //}
 
    }
}
