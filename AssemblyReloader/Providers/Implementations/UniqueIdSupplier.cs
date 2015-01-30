namespace AssemblyReloader.Providers.Implementations
{
    public class UniqueIdSupplier
    {
        private static int _id = 0;

        public int Generate()
        {
            return _id++;
        }
    }
}
