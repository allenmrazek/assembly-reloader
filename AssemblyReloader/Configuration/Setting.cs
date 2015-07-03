namespace AssemblyReloader.Configuration
{
    public class Setting<T> : ISetting<T>
    {
        private T _value;


        public Setting() : this(default(T))
        {
        }


        public Setting(T value)
        {
            _value = value;
        }


        public T Get()
        {
            return _value;
        }


        public void Set(T value)
        {
            _value = value;
        }
    }
}
