using System.Collections;
using UnityEngine;

namespace AssemblyReloader
{
    public interface IRoutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine routine);
    }
}
