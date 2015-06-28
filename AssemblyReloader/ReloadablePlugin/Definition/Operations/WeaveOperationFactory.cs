using System;
using System.Collections.Generic;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Generators;
using AssemblyReloader.Properties;

namespace AssemblyReloader.ReloadablePlugin.Definition.Operations
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
                new RenameAssemblyOperation(_uniqueAssemblyNameGenerator)
            };
        }
    }
}
