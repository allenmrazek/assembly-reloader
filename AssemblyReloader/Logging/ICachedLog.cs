using ReeperCommon.Logging;

namespace AssemblyReloader.Logging
{
    interface ICachedLog : ILog
    {
        string[] Messages { get; }
    }
}
