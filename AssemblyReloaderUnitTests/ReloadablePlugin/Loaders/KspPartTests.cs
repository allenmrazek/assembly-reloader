using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.ReloadablePlugin.Loaders;
using NSubstitute;
using Xunit;
// ReSharper disable once CheckNamespace
namespace AssemblyReloader.ReloadablePlugin.Loaders.Tests
{
    public class KspPartTests
    {
        class KspPartSimilar : IEquatable<KspPartSimilar>
        {
            private readonly IPart _someShared;

            public KspPartSimilar(IPart someShared)
            {
                if (someShared == null) throw new ArgumentNullException("someShared");
                _someShared = someShared;
            }

            public bool Equals(KspPartSimilar other)
            {
                return other != null && ReferenceEquals(_someShared, other._someShared);
            }

            //bool IEquatable<KspPartSimilar>.Equals(KspPartSimilar other)
            //{
            //    return Equals(other);
            //}

            public override bool Equals(object obj)
            {
                return Equals(obj as KspPartSimilar);
            }


            public override int GetHashCode()
            {
                return _someShared.GetHashCode();
            }
        }

        [Fact()]
        public void KspPart_EqualityTest_Concrete()
        {
            var p = Substitute.For<IPart>();

            var sut1 = new KspPartSimilar(p);
            var sut2 = new KspPartSimilar(p);

            Assert.True(sut1.Equals(sut2));
        }

        [Fact()]
        public void KspPart_EqualityTest_Interfaces()
        {
            var p = Substitute.For<IPart>();

            IEquatable<KspPartSimilar> sut1 = new KspPartSimilar(p);
            IEquatable<KspPartSimilar> sut2 = new KspPartSimilar(p);

            Assert.True(sut1.Equals(sut2));
        }
    }
}
