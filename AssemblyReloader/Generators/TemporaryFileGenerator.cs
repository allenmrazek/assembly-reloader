using System;
using System.Globalization;
using System.IO;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Generators
{
    public class TemporaryFileGenerator : ITemporaryFileGenerator
    {
        private readonly IDirectory _directory;
        private readonly IRandomStringGenerator _stringGenerator;

        public TemporaryFileGenerator(
            [NotNull] IDirectory directory,
            [NotNull] IRandomStringGenerator stringGenerator)
        {
            if (directory == null) throw new ArgumentNullException("directory");
            if (stringGenerator == null) throw new ArgumentNullException("stringGenerator");
            _directory = directory;
            _stringGenerator = stringGenerator;
        }


        public TemporaryFile Get()
        {
            string fullPath;

            do
            {
                fullPath = GenerateFilePath();
            } while (File.Exists(fullPath));

            return new TemporaryFile(fullPath);
        }


        private string GenerateFilePath()
        {
            return _directory.FullPath +
                   (!_directory.FullPath.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture))
                       ? Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)
                       : "")
                   + _stringGenerator.Get();
        }
    }
}
