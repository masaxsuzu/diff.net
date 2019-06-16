
using System;

using Xunit;
namespace NetSoft.Algorithms.Tests
{
    public class DPTest
    {
        public static object[][] Strings()
        {
            return new object[][]{
                new object[]{
                    new string[]{"h", "e" },
                    new string[]{"s","h", "e" },
                    new EditScript<string>(){
                        new Edit<string>(){Change = 1, Value = "s" },
                        new Edit<string>(){Change = 0, Value = "h" },
                        new Edit<string>(){Change = 0, Value = "e" },
                    }
                },
                new object[]{
                    new string[]{"s","h", "e" },
                    new string[]{   "h", "e" },
                    new EditScript<string>(){
                        new Edit<string>(){Change = -1, Value =  "s" },
                        new Edit<string>(){Change = 0, Value =   "h" },
                        new Edit<string>(){Change = 0, Value =   "e" },
                    }
                },
                new object[]{
                    new string[]{      "x=", "1*", "3", ";"},
                    new string[]{"let","x=", "1*",       ";"},
                    new EditScript<string>(){
                        new Edit<string>(){Change = 1, Value =  "let" },
                        new Edit<string>(){Change = 0, Value =  "x=" },
                        new Edit<string>(){Change = 0, Value =  "1*" },
                        new Edit<string>(){Change = -1, Value =  "3" },
                        new Edit<string>(){Change = 0, Value =  ";" },
                    }
                },
                new object[]{

                    new string[]{"a","b", "c" },
                    new string[]{"a","x","c","d" },

                    new EditScript<string>(){
                        new Edit<string>(){Change = 0, Value =  "a" },
                        new Edit<string>(){Change = -1, Value =  "b" },
                        new Edit<string>(){Change = 1, Value =  "x" },
                        new Edit<string>(){Change = 0, Value =  "c" },
                        new Edit<string>(){Change = 1, Value =  "d" },
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
                    new EditScript<int>(){
                        new Edit<int>(){Change = -1, Value =  1 },
                        new Edit<int>(){Change =  0, Value =  2 },
                        new Edit<int>(){Change =  0, Value =  3 },
                        new Edit<int>(){Change = 1, Value =  6 },
                    }
                },
            };
        }

        [Theory]
        [MemberData(nameof(Strings))]
        public void DiffString(string[] x, string[] y, EditScript<string> want)
        {
            Diff(x, y, want);
        }

        [Theory]
        [MemberData(nameof(Integers))]
        public void DiffIneger(int[] x, int[] y, EditScript<int> want)
        {
            Diff(x, y, want);
        }

        private void Diff<T>(T[] x, T[] y, EditScript<T> want)
            where T : IEquatable<T>
        {
            var got = DP<T>.Diff(x, y);

            AssertDifference<T>(want, got);
        }
        private void AssertDifference<T>(EditScript<T> want, EditScript<T> got) where T : IEquatable<T>
        {
            Xunit.Assert.Equal(want.Count, got.Count);
            for (int i = 0; i < want.Count; i++)
            {
                var x = want[i];
                var y = got[i];
                Xunit.Assert.Equal(x.Change, x.Change);
                Xunit.Assert.Equal(x.Value, y.Value);
            }
        }
    }
}
