//using System;
//using System.Linq;
//using System.Reflection;
//using UnityEngine;

//namespace AssemblyReloader.Destruction
//{
//    // inform the target (generic GameObject) that we'll be destroying it to make way for
//    // a new instance from an assembly that is about to be loaded
//    class GameObjectDestroyForReload : IDestructionMediator
//    {
//        private const string DestructionMethodName = "OnPluginReloadRequested";

//        public void InformTargetOfDestruction(GameObject target)
//        {
//            if (target == null) throw new ArgumentNullException("target");

//            target.GetComponentsInChildren<Component>(true).ToList().ForEach(c => InformComponentOfDestruction(c));
//        }


//        public void InformComponentOfDestruction(Component component)
//        {
//            if (component == null) throw new ArgumentNullException("component");

//            var method = GetMethod(DestructionMethodName, component.GetType());

//            if (method != null) // not having such a method is valid
//                method.Invoke(component, new object[]{});
//        }


//        private MethodInfo GetMethod(string name, Type type)
//        {
//            if (type == null) throw new ArgumentNullException("type");

//            return type.GetMethod(name,
//                BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic,
//                null, new Type[] {}, null);
//        }
//    }
//}
