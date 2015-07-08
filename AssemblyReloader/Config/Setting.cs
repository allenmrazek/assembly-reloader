using System;
using ReeperCommon.Logging;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Config
{
    public class Setting<T> : ISetting<T>
    {
        [ReeperPersistent] public T _value;


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


        public static implicit operator Setting<T>(T from)
        {
            return new Setting<T>(@from);
        }

        public static implicit operator T(Setting<T> from)
        {
            if (@from == null) throw new ArgumentNullException("from");

            return from._value;
        }


        public void Serialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            new DebugLog().Normal("Serializing " + _value);
            formatter.Serialize(_value, node);

            new DebugLog().Normal("Result: {0}", node.ToString());
        }


        public void Deserialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            formatter.Deserialize(_value, node);
        }
    }
}
