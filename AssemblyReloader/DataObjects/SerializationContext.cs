using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.Providers;

namespace AssemblyReloader.DataObjects
{
    public class SerializationContext
    {
        public object Target { get; private set; }
        public IFilePathProvider PathProvider { get; private set; }

        public SerializationContext([NotNull] object target, [NotNull] IFilePathProvider pathProvider)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (pathProvider == null) throw new ArgumentNullException("pathProvider");

            Target = target;
            PathProvider = pathProvider;
        }
    }
}
