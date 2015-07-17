using System;
using System.Globalization;
using System.IO;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Properties;
using AssemblyReloader.ReloadablePlugin.Weaving;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.FileSystem
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetTemporaryFile : IGetTemporaryFile
    {
        private readonly IDirectory _directory;
        private readonly IGetRandomString _string;

        public GetTemporaryFile(
            [NotNull] IDirectory directory,
            [NotNull] IGetRandomString @string)
        {
            if (directory == null) throw new ArgumentNullException("directory");
            if (@string == null) throw new ArgumentNullException("string");
            _directory = directory;
            _string = @string;
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
                   + _string.Get();
        }
    }
}
