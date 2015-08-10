using System;
using System.IO;

namespace AssemblyReloader.FileSystem
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

            _stream = new FileStream(_fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (_stream == null)
                throw new FileLoadException("Failed to open file stream at " + _fullPath);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        ~TemporaryFile()
        {
            Dispose(false);
        }


        private void Dispose(bool managed)
        {
            if (managed)
            {
                if (_stream != null) _stream.Dispose();
            }

            if (!File.Exists(_fullPath)) return;

            File.Delete(_fullPath);
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
