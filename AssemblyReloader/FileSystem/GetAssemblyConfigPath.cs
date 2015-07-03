using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Properties;
using AssemblyReloader.Queries.FileSystemQueries;

namespace AssemblyReloader.FileSystem
{
    public class GetAssemblyConfigPath : IFilePathProvider<Assembly>
    {
        private const string ProgramConfigurationFileExtension = ".config";

        private readonly IGetAssemblyFileLocation _getAssemblyLocation;
        

        public GetAssemblyConfigPath([NotNull] IGetAssemblyFileLocation getAssemblyLocation)
        {
            if (getAssemblyLocation == null) throw new ArgumentNullException("getAssemblyLocation");
            _getAssemblyLocation = getAssemblyLocation;

        }


        public string Get(Assembly context)
        {
            if (context == null) throw new ArgumentNullException("context");

            var mf =_getAssemblyLocation.Get(context);

            if (!mf.Any())
                throw new Exception("Could not find file location of " + context.FullName);

            return Path.Combine(Path.GetDirectoryName(mf.Single().FullPath) ?? string.Empty, Path.GetFileNameWithoutExtension(mf.Single().FullPath) + ProgramConfigurationFileExtension);
        }
    }
}
