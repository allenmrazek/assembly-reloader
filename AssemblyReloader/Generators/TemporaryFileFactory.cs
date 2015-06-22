using System;
using System.Globalization;
using System.IO;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Generators
{
    public class TemporaryFileFactory : ITemporaryFileFactory
    {
        private readonly IDirectory _directory;
        private readonly IRandomStringGenerator _stringGenerator;


        public TemporaryFileFactory(
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


        public TemporaryFile Get(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
                throw new ArgumentException("argument cannot be null or empty");

            if (File.Exists(fullPath))
                throw new ArgumentException("A file already exists at " + fullPath);

            if (string.IsNullOrEmpty(Path.GetFileName(fullPath)))
                throw new ArgumentException("must be a file name, not a directory path");

            if (Directory.Exists(fullPath))
                throw new ArgumentException("a directory exists at " + fullPath);

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
