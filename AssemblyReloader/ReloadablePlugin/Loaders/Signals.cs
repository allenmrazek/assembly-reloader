using AssemblyReloader.StrangeIoC.extensions.signal.impl;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public class SignalAboutToDestroyMonoBehaviour : Signal<MonoBehaviour>
    {

    }

    public class SignalLoadersFinished : Signal
    {
    }
}
