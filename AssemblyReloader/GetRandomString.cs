using System;

namespace AssemblyReloader
{
    public class GetRandomString : IGetRandomString
    {
        public string Get()
        {
            return Guid.NewGuid().ToString("n");
        }
    }
}
