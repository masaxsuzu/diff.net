using System;
using System.Linq;

using Xunit;

namespace Netsoft.Diff.Tests
{
    public class DiffTest
    {
        [Theory]
        [Trait("Category", "SurrogatePair")]
        [MemberData(nameof(TestInput.StringWithSurrogatePair), MemberType = typeof(TestInput))]
        public void TestDiff(string x, string y, IChangeCollection<string> want)
        {
            var got = Differences.Between(x, y);
            AssertDifference<string>(want, got);
        }

        [Theory]
        [Trait("Category", "String")]
        [MemberData(nameof(TestInput.Strings), MemberType = typeof(TestInput))]
        public void TestDiffByStrings(string[] x, string[] y, IChangeCollection<string> want)
        {
            var got = Differences.Between(x, y);
            AssertDifference<string>(want, got);
        }

        [Theory]
        [Trait("Category", "Integer")]
        [MemberData(nameof(TestInput.Integers), MemberType = typeof(TestInput))]
        public void TestDiffByIntegers(int[] x, int[] y, IChangeCollection<int> want)
        {
            var got = Differences.Between(x, y);
            AssertDifference<int>(want, got);
        }

        [Theory]
        [Trait("Category", "Null")]
        [MemberData(nameof(TestInput.Nulls), MemberType = typeof(TestInput))]
        public void TestDiffThrowsExceptionIfNullArgument(string[] x, string[] y)
        {
            var error = Assert.Throws<ArgumentNullException>(() => _ = Differences.Between(x, y));
        }

        [Fact]
        public void TestDiffCustom()
        {
            var diff = Differences.Between(
                new X[] {
                    new X() { V = 1},
                    new X() { V = 2},
                    new X() { V = 0},
                    new X() { V = 1},
                },
                new X[] {
                    new X() { V = 22},
                    new X() { V = 2},
                    new X() { V = 1},
                });

            Xunit.Assert.Equal(4, diff.Count);
            Xunit.Assert.Equal(3, diff.Distance);

        }

        private void AssertDifference<T>(IChangeCollection<T> w, IChangeCollection<T> g) where T : IEquatable<T>
        {
            var want = w.ToArray();
            var got = g.ToArray();

            Xunit.Assert.Equal(w.Distance, g.Distance);
            Xunit.Assert.Equal(want.Length, got.Length);
            for (int i = 0; i < want.Length; i++)
            {
                var x = want[i];
                var y = got[i];
                Xunit.Assert.Equal(x.Action, y.Action);
                Xunit.Assert.Equal(x.Value, y.Value);
                Xunit.Assert.Equal(x, y);
            }
        }
    }
    class X : IEquatable<X>
    {
        public int V { get; set; }
        public bool Equals(X other)
        {
            return V == other.V;
        }

        public override string ToString()
        {
            return $"{V}";
        }
    }
}
