//using System;
//namespace AssemblyReloader.Config
//{
//    public class Setting<T> : ISetting<T>
//    {
//// ReSharper disable once MemberCanBePrivate.Global
//        public T Value;


//        public Setting() : this(default(T))
//        {
//        }


//        public Setting(T value)
//        {
//            Value = value;
//        }


//        public T Get()
//        {
//            return Value;
//        }


//        public void Set(T value)
//        {
//            Value = value;
//        }


//        public static implicit operator Setting<T>(T from)
//        {
//            return new Setting<T>(@from);
//        }

//        public static implicit operator T(Setting<T> from)
//        {
//            if (@from == null) throw new ArgumentNullException("from");

//            return from.Value;
//        }
//    }
//}
