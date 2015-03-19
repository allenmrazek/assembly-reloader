using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.CompositeRoot.Commands;
using Object = UnityEngine.Object;

namespace AssemblyReloader.Destruction
{
    public class UnityObjectDestroyer : IUnityObjectDestroyer
    {
        private readonly ICommand<Object> _executeBeforeDestruction;

        public UnityObjectDestroyer(ICommand<UnityEngine.Object> executeBeforeDestruction)
        {
            if (executeBeforeDestruction == null) throw new ArgumentNullException("executeBeforeDestruction");
            _executeBeforeDestruction = executeBeforeDestruction;
        }


        public void Destroy<T>(T target) where T : Object
        {
            if (target == null) throw new ArgumentNullException("target");

            _executeBeforeDestruction.Execute(target);

            UnityEngine.Object.Destroy(target);
        }
    }
}
