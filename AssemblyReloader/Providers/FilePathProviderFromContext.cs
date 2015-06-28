using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.FileSystem;

namespace AssemblyReloader.Providers
{
    public class FilePathProviderFromContext<T> : IFilePathProvider
    {
        private readonly Func<T> _contextProvider;
        private readonly IFilePathProvider<T> _requiresContext;

        public FilePathProviderFromContext([NotNull] Func<T> contextProvider,
            [NotNull] IFilePathProvider<T> requiresContext)
        {
            if (contextProvider == null) throw new ArgumentNullException("contextProvider");
            if (requiresContext == null) throw new ArgumentNullException("requiresContext");

            _contextProvider = contextProvider;
            _requiresContext = requiresContext;
        }


        public string Get()
        {
            return _requiresContext.Get(_contextProvider());
        }
    }
}
