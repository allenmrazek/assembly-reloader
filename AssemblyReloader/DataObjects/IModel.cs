using System.Collections.Generic;
using AssemblyReloader.Controllers;

namespace AssemblyReloader.DataObjects
{
    public interface IModel
    {
        IEnumerable<IReloadablePlugin> Plugins { get; }
        Configuration Configuration { get; }
    }
}
