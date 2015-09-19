using strange.extensions.signal.impl;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class SignalAboutToDestroyMonoBehaviour : Signal<MonoBehaviour>
    {

    }

// ReSharper disable once ClassNeverInstantiated.Global
    public class SignalLoadersFinished : Signal
    {
    }
}
