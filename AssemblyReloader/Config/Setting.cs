using System;
using ReeperCommon.Logging;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Config
{
    public class Setting<T> : ISetting<T>//, IReeperPersistent
    {
// ReSharper disable once MemberCanBePrivate.Global
        /*[ReeperPersistent]*/ public T Value;


        public Setting() : this(default(T))
        {
        }


        public Setting(T value)
        {
            Value = value;
        }


        public T Get()
        {
            return Value;
        }


        public void Set(T value)
        {
            Value = value;
        }


        public static implicit operator Setting<T>(T from)
        {
            return new Setting<T>(@from);
        }

        public static implicit operator T(Setting<T> from)
        {
            if (@from == null) throw new ArgumentNullException("from");

            return from.Value;
        }


        //public void Serialize(IConfigNodeSerializer formatter, ConfigNode node)
        //{
        //    new DebugLog().Normal("Serializing " + Value);
        //    formatter.Serialize(Value, node);

        //    new DebugLog().Normal("Result: {0}", node.ToString());
        //}


        //public void Deserialize(IConfigNodeSerializer formatter, ConfigNode node)
        //{
        //    formatter.Deserialize(Value, node);
        //}
    }
}
