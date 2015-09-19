using System;

namespace AssemblyReloader.Config
{
    public class GuiSkinNotFoundException : Exception
    {
        public GuiSkinNotFoundException() : base("GUISkin not found")
        {
            
        }


        public GuiSkinNotFoundException(string skin) : base("GUISkin \"" + skin + "\" not found")
        {
            
        }


        public GuiSkinNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
