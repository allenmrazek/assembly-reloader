using ReeperCommon.Logging;

namespace AssemblyReloader.CompositeRoot
{
    interface ICachedLog : ILog
    {
        string[] Messages { get; }
    }
}
