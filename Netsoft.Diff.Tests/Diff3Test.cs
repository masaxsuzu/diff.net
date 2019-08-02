using System;
using System.Linq;

using Xunit;
namespace Netsoft.Diff.Tests
{
    public class Diff3Test
    {
        [Theory]
        [Trait("Category", "Diff3")]
        [MemberData(nameof(TestInput.Diff3), MemberType = typeof(TestInput))]
        public void TestDiff3(string a, string b, string c, IEditScript<IEdit<string>> want)
        {
            string[] x = a.Split(new char[] { '\n' });
            string[] y = b.Split(new char[] { '\n' });
            string[] z = c.Split(new char[] { '\n' });

            var diff3 = x.Diff3(y, z);
            Assert(want, diff3);
        }

        [Theory]
        [Trait("Category", "Diff3")]
        [MemberData(nameof(TestInput.StringDiff3), MemberType = typeof(TestInput))]
        public void TestStringDiff3(string a, string b, string c, IEditScript<IEdit<string>> want)
        {
            var diff3 = a.Diff3(b, c);
            Assert(want, diff3);
        }

        private static void Assert<T>(IEditScript<IEdit<T>> want, IEditScript<IEdit<T>> diff3) where T : IEquatable<T>
        {
            Xunit.Assert.Equal(want.Distance, diff3.Distance);
            Xunit.Assert.Equal(want.Count, diff3.Count);
            foreach (var (First, Second) in want.Zip(diff3))
            {
                Xunit.Assert.Equal(First, Second);
            }
        }

    }
}
