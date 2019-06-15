
using System;

using Xunit;
namespace NetSoft.Algorithms.Tests
{
    public class SESTest
    {
        public static object[][] Strings()
        {
            return new object[][]{
                new object[]{
                    new string[]{"h", "e" },
                    new string[]{"s","h", "e" },
                    new Difference<string>(){
                        new Edit<string>(){Change = 1, Value = new string[]{ "s" } },
                        new Edit<string>(){Change = 0, Value = new string[]{ "h" } },
                        new Edit<string>(){Change = 0, Value = new string[]{ "e" } },
                    }
                },
                new object[]{
                    new string[]{"s","h", "e" },
                    new string[]{   "h", "e" },
                    new Difference<string>(){
                        new Edit<string>(){Change = -1, Value = new string[]{ "s" } },
                        new Edit<string>(){Change = 0, Value =  new string[]{ "h" } },
                        new Edit<string>(){Change = 0, Value =  new string[]{ "e" } },
                    }
                },
                new object[]{
                    new string[]{      "x=", "1*", "3", ";"},
                    new string[]{"let","x=", "1*",       ";"},
                    new Difference<string>(){
                        new Edit<string>(){Change = 1, Value = new string[]{ "let" } },
                        new Edit<string>(){Change = 0, Value = new string[]{ "x=" } },
                        new Edit<string>(){Change = 0, Value = new string[]{ "1*" } },
                        new Edit<string>(){Change = -1, Value = new string[]{ "3" } },
                        new Edit<string>(){Change = 0, Value = new string[]{ ";" } },
                    }
                },
            };
        }
        public static object[][] Integers()
        {
            return new object[][]{
                new object[]{
                    new int[]{ 1,2,3 },
                    new int[]{ 2,3,6},
                    new Difference<int>(){
                        new Edit<int>(){Change = -1, Value = new int[]{ 1 } },
                        new Edit<int>(){Change =  0, Value = new int[]{ 2 } },
                        new Edit<int>(){Change =  0, Value = new int[]{ 3 } },
                        new Edit<int>(){Change = 1, Value = new int[]{ 6 } },
                    }
                },
            };
        }

        [Theory]
        [MemberData(nameof(Strings))]
        public void DiffString(string[] x, string[] y, Difference<string> want)
        {
            Diff(x, y, want);
        }

        [Theory]
        [MemberData(nameof(Integers))]
        public void DiffIneger(int[] x, int[] y, Difference<int> want)
        {
            Diff(x, y, want);
        }

        private void Diff<T>(T[] x, T[] y, Difference<T> want)
            where T : IEquatable<T>
        {
            var got = SES<T>.Diff(x, y);

            Xunit.Assert.Equal(want, got);
        }

    }
}
