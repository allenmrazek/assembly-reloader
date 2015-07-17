using System;
using System.Collections.Generic;
using AssemblyReloader.Config;
using AssemblyReloader.Properties;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old.Definition.Operations
{
    [Implements(typeof(IWeaveOperationFactory))]
// ReSharper disable once UnusedMember.Global
    public class WeaveOperationFactory : IWeaveOperationFactory
    {
        private readonly IUniqueAssemblyNameGenerator _uniqueAssemblyNameGenerator;

        public WeaveOperationFactory([NotNull] IUniqueAssemblyNameGenerator uniqueAssemblyNameGenerator)
        {
            if (uniqueAssemblyNameGenerator == null) throw new ArgumentNullException("uniqueAssemblyNameGenerator");

            _uniqueAssemblyNameGenerator = uniqueAssemblyNameGenerator;
        }


        public IEnumerable<IWeaveOperation> Create(PluginConfiguration pluginConfiguration)
        {
            return new IWeaveOperation[]
            {
                //new RenameAssemblyOperation(_uniqueAssemblyNameGenerator)
            };
        }
    }
}
