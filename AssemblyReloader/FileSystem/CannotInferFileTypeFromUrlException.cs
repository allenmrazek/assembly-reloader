using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.FileSystem
{
    public class CannotInferFileTypeFromUrlException : Exception
    {
        public CannotInferFileTypeFromUrlException() : base("Can't infer file type from URL")
        {
            
        }

        public CannotInferFileTypeFromUrlException(string url) : base("Can't infer file type from url \"" + url + "\"")
        {
            
        }

        public CannotInferFileTypeFromUrlException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}
