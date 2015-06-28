//using System;
//using AssemblyReloader.Annotations;
//using AssemblyReloader.Queries.FileSystemQueries;

//namespace AssemblyReloader.Commands
//{
//    public class SaveConfigNodeFromCallbacksCommand : ICommand
//    {
//        private readonly string _nodeName;
//        private readonly IFilePathProvider _filePathQuery;
//        private readonly string _header;
//        public event Action<ConfigNode> OnExecute = delegate { };

//        public SaveConfigNodeFromCallbacksCommand([NotNull] string nodeName, [NotNull] IFilePathProvider filePathQuery,
//            [NotNull] string header = "")
//        {
//            if (nodeName == null) throw new ArgumentNullException("nodeName");
//            if (filePathQuery == null) throw new ArgumentNullException("filePathQuery");
//            if (header == null) throw new ArgumentNullException("header");
//            if (string.IsNullOrEmpty(nodeName)) nodeName = "root";

//            _nodeName = nodeName;
//            _filePathQuery = filePathQuery;
//            _header = header;
//        }

//        public void Execute()
//        {
//            var config = new ConfigNode(_nodeName);

//            OnExecute(config);

//            var path = _filePathQuery.Get();

//            if (!string.IsNullOrEmpty(_header))
//                config.Serialize(path, _header);
//            else config.Serialize(path);
//        }
//    }
//}
