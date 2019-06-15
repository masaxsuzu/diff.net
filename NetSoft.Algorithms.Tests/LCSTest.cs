using Xunit;

namespace NetSoft.Algorithms.Tests
{
    public class LCSTest
    {
        [Theory]
        [InlineData("", "", "")]
        public void TestDiff(string x, string y, string want)
        {
            string got = LCS.Diff(x, y);

            Xunit.Assert.Equal(want, got);
        }
    }
}
