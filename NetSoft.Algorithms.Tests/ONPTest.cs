using System;

using Xunit;

namespace NetSoft.Algorithms.Tests
{
    public class ONPTest
    {
        [Theory]
        [Trait("Category", "String")]
        [MemberData(nameof(TestInput.Strings), MemberType = typeof(TestInput))]
        public void DiffString(string[] x, string[] y, EditScript<string> want)
        {
            Diff(x, y, want);
        }

        [Theory]
        [Trait("Category", "Integer")]
        [MemberData(nameof(TestInput.Integers), MemberType = typeof(TestInput))]
        public void DiffIneger(int[] x, int[] y, EditScript<int> want)
        {
            Diff(x, y, want);
        }

        [Theory]
        [Trait("Category", "Benchmark")]
        [MemberData(nameof(BenchmarkInput.x500), MemberType = typeof(BenchmarkInput))]
        [MemberData(nameof(BenchmarkInput.x1000), MemberType = typeof(BenchmarkInput))]
        [MemberData(nameof(BenchmarkInput.x2000), MemberType = typeof(BenchmarkInput))]
        public void Benchmark(int[] x, int[] y, EditScript<int> want)
        {
            Diff(x, y, want);
        }
        private void Diff<T>(T[] x, T[] y, EditScript<T> want)
            where T : IEquatable<T>
        {
            var got = x.sDiff(y);

            AssertDifference<T>(want.ToArray(), got.ToArray());
        }
        private void AssertDifference<T>(Edit<T>[] want, Edit<T>[] got) where T : IEquatable<T>
        {
            Xunit.Assert.Equal(want.Length, got.Length);
            for (int i = 0; i < want.Length; i++)
            {
                var x = want[i];
                var y = got[i];
                Xunit.Assert.Equal(x.Change, y.Change);
                Xunit.Assert.Equal(x.Value, y.Value);
            }
        }

    }
}
