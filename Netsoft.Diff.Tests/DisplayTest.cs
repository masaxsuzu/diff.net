using Xunit;

namespace Netsoft.Diff.Tests
{
    public class DisplayTest
    {
        [Theory]
        [Trait("Category", "Show")]
        [MemberData(nameof(TestInput.Show), MemberType = typeof(TestInput))]
        public void Show(string want, IEditScript<string> input)
        {
            string got = input.Show<string>();

            Assert.Equal(want, got);
        }
    }
}
