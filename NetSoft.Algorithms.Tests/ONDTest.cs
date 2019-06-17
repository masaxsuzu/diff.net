using System;

using Xunit;

namespace NetSoft.Algorithms.Tests
{
    public class ONDTest
    {
        [Theory]
        [MemberData(nameof(TestInput.Strings), MemberType = typeof(TestInput))]
        public void DiffString(string[] x, string[] y, EditScript<string> want)
        {
            Diff(x, y, want);
        }

        [Theory]
        [MemberData(nameof(TestInput.Integers), MemberType = typeof(TestInput))]
        public void DiffIneger(int[] x, int[] y, EditScript<int> want)
        {
            Diff(x, y, want);
        }

        private void Diff<T>(T[] x, T[] y, EditScript<T> want)
            where T : IEquatable<T>
        {
            var got = x.xDiff(y);

            AssertDifference<T>(want, got);
        }
        private void AssertDifference<T>(EditScript<T> want, EditScript<T> got) where T : IEquatable<T>
        {
            Xunit.Assert.Equal(want.Count, got.Count);
            for (int i = 0; i < want.Count; i++)
            {
                var x = want[i];
                var y = got[i];
                Xunit.Assert.Equal(x.Change, y.Change);
                Xunit.Assert.Equal(x.Value, y.Value);
            }
        }

    }
}
