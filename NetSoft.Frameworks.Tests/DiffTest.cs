using System;
using System.Linq;

using Xunit;

namespace NetSoft.Frameworks.Tests
{
    public class DiffTest
    {
        [Theory]
        [Trait("Category", "SurrogatePair")]
        [MemberData(nameof(TestInput.StringWithSurrogatePair), MemberType = typeof(TestInput))]
        public void TestDiff(string x, string y, IEditScript<string> want)
        {
            var got = x.Diff(y);
            AssertDifference<string>(want, got);
        }

        [Theory]
        [Trait("Category", "String")]
        [MemberData(nameof(TestInput.Strings), MemberType = typeof(TestInput))]
        public void TestDiffByStrings(string[] x, string[] y, IEditScript<string> want)
        {
            Diff(x, y, want);
        }

        [Theory]
        [Trait("Category", "Null")]
        [MemberData(nameof(TestInput.Nulls), MemberType = typeof(TestInput))]
        public void TestDiffThrowsExceptionIfNullArgument(string[] x, string[] y)
        {
            var error = Assert.Throws<ArgumentNullException>(() => _ = x.Diff(y));
        }

        [Theory]
        [Trait("Category", "Integer")]
        [MemberData(nameof(TestInput.Integers), MemberType = typeof(TestInput))]
        public void TestDiffByIntegers(int[] x, int[] y, IEditScript<int> want)
        {
            Diff(x, y, want);
        }


        private void Diff<T>(T[] x, T[] y, IEditScript<T> want)
            where T : IEquatable<T>
        {
            var got = x.Diff(y);

            AssertDifference<T>(want, got);
        }
        private void AssertDifference<T>(IEditScript<T> w, IEditScript<T> g) where T : IEquatable<T>
        {
            var want = w.ToArray();
            var got = g.ToArray();

            //Xunit.Assert.Equal(w.Distance, g.Distance);
            Xunit.Assert.Equal(want.Length, got.Length);
            for (int i = 0; i < want.Length; i++)
            {
                var x = want[i];
                var y = got[i];
                Xunit.Assert.Equal(x.Action, y.Action);
                Xunit.Assert.Equal(x.Value, y.Value);
            }
        }
    }
}