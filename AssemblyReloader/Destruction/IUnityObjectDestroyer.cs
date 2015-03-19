using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Destruction
{
    public interface IUnityObjectDestroyer
    {
        void Destroy<T>(T target) where T : UnityEngine.Object;
    }
}
