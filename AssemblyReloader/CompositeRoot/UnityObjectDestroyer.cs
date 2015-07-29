using System;
using AssemblyReloader.Commands;
using AssemblyReloader.Commands.old;
using Object = UnityEngine.Object;

namespace AssemblyReloader.CompositeRoot
{
    public class UnityObjectDestroyer : IUnityObjectDestroyer
    {
        private readonly ICommand<Object> _executeBeforeDestruction;

        public UnityObjectDestroyer(ICommand<Object> executeBeforeDestruction)
        {
            if (executeBeforeDestruction == null) throw new ArgumentNullException("executeBeforeDestruction");
            _executeBeforeDestruction = executeBeforeDestruction;
        }


        public void Destroy<T>(T target) where T : Object
        {
            if (target == null) throw new ArgumentNullException("target");

            _executeBeforeDestruction.Execute(target);

            Object.DestroyImmediate(target);
        }
    }
}
