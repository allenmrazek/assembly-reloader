using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Logging;

namespace AssemblyReloader.Logging
{
    interface ICachedLog : ILog
    {
        string[] Messages { get; }
    }
}
