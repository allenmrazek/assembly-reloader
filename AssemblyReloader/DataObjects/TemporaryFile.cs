using System;
using System.IO;
using System.Security;
using KSPAchievements;
using ReeperCommon.Logging.Implementations;

namespace AssemblyReloader.DataObjects
{
    public class TemporaryFile : IDisposable
    {
        private readonly string _fullPath;
        private FileStream _stream;

        public TemporaryFile(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
                throw new ArgumentException("fullPath not set");

            if (File.Exists(fullPath))
                throw new ArgumentException("File already exists at " + fullPath);

            _fullPath = fullPath;
        }


        private void LazyInitialize()
        {
            if (_stream != null) return;

            _stream = new FileStream(_fullPath, FileMode.CreateNew, FileAccess.ReadWrite);
            if (_stream == null)
                throw new FileLoadException("Failed to open file stream at " + _fullPath);
        }


        public void Dispose()
        {
            if (_stream != null) _stream.Dispose();
            if (!File.Exists(_fullPath)) return;

            // todo: remove
            var log = new DebugLog("TemporaryFile");

            log.Normal("Deleting " + FullPath);
            File.Delete(_fullPath);
            log.Normal("done");
        }


        public Stream Stream
        {
            get
            {
                LazyInitialize();

                return _stream;
            }
        }


        public string FullPath
        {
            get { return _fullPath; }
        }
    }
}
