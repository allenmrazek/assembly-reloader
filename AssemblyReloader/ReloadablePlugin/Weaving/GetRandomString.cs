using System;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public class GetRandomString : IGetRandomString
    {
        public string Get()
        {
            return Guid.NewGuid().ToString("n");
        }
    }
}
