using Netsoft.Diff.Tests;

using System;
using System.Linq;

using Xunit;

namespace Netsoft.Diff.Algorithms.Tests
{
    public class ONDTest
    {
        [Theory]
        [Trait("Category", "String")]
        [MemberData(nameof(TestInput.Strings), MemberType = typeof(TestInput))]
        public void DiffString(string[] x, string[] y, IEditScript<string> want)
        {
            Diff(x, y, want);
        }

        [Theory]
        [Trait("Category", "Integer")]
        [MemberData(nameof(TestInput.Integers), MemberType = typeof(TestInput))]
        public void DiffIneger(int[] x, int[] y, IEditScript<int> want)
        {
            Diff(x, y, want);
        }

        [Theory]
        [Trait("Category", "ContainsNull")]
        [MemberData(nameof(TestInput.ContainsNull), MemberType = typeof(TestInput))]
        public void TestDiffHandlesSequenceCotainingNull(string[] x, string[] y, IEditScript<string> want)
        {
            Diff(x, y, want);
        }

        [Theory]
        [Trait("Category", "Benchmark")]
        [MemberData(nameof(BenchmarkInput.x500), MemberType = typeof(BenchmarkInput))]
        [MemberData(nameof(BenchmarkInput.x1000), MemberType = typeof(BenchmarkInput))]
        [MemberData(nameof(BenchmarkInput.x2000), MemberType = typeof(BenchmarkInput))]
        public void Benchmark(int[] x, int[] y, IEditScript<int> want)
        {
            Diff(x, y, want);
        }
        private void Diff<T>(T[] x, T[] y, IEditScript<T> want)
            where T : IEquatable<T>
        {
            var got = Netsoft.Diff.Algorithms.OND.Diff(x, y);

            AssertDifference<T>(want, got);
        }
        private void AssertDifference<T>(IEditScript<T> w, IEditScript<T> g) where T : IEquatable<T>
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
            }
        }

    }
}
