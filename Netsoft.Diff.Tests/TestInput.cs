using System;
using System.Collections.Generic;
using System.Linq;

namespace Netsoft.Diff.Tests
{
    public class TestInput
    {
        public static IEnumerable<object[]> Strings()
        {
            yield return new object[]{
                    new string[]{"h", "e" },
                    new string[]{"s","h", "e" },
                    new EditScript<string>(0){
                        new Edit<string>(){Action = 0, Value = "e" },
                        new Edit<string>(){Action = 0, Value = "h" },
                        new Edit<string>(){Action = 1, Value = "s" },
                    }
                };
            yield return new object[]{
                    new string[]{"s","h", "e" },
                    new string[]{   "h", "e" },
                    new EditScript<string>(0){
                        new Edit<string>(){Action = 0, Value =   "e" },
                        new Edit<string>(){Action = 0, Value =   "h" },
                        new Edit<string>(){Action = -1, Value =  "s" },
                    }
                };
            yield return new object[]{
                    new string[]{      "x=", "1*", "3", ";"},
                    new string[]{"let","x=", "1*",       ";"},
                    new EditScript<string>(0){
                        new Edit<string>(){Action = 0, Value =  ";" },
                        new Edit<string>(){Action = -1, Value =  "3" },
                        new Edit<string>(){Action = 0, Value =  "1*" },
                        new Edit<string>(){Action = 0, Value =  "x=" },
                        new Edit<string>(){Action = 1, Value =  "let" },
                    }
                };
            yield return new object[]{
                    new string[]{"a","b", "c" },
                    new string[]{"a","x","c","d" },

                    new EditScript<string>(1){
                        new Edit<string>(){Action = 1, Value =  "d" },
                        new Edit<string>(){Action = 0, Value =  "c" },
                        new Edit<string>(){Action = 2, Value =  "x", From = "b" },
                        new Edit<string>(){Action = 0, Value =  "a" },
                    }
                };
            yield return new object[]{
                    new string[]{"k","i","t","t","e","n" },
                    new string[]{"s","i","t","t","i","n","g" },

                    new EditScript<string>(2){
                        new Edit<string>(){Action = 1, Value =  "g" },
                        new Edit<string>(){Action = 0, Value =  "n" },
                        new Edit<string>(){Action = 2, Value =  "i", From = "e" },
                        new Edit<string>(){Action = 0, Value =  "t" },
                        new Edit<string>(){Action = 0, Value =  "t" },
                        new Edit<string>(){Action = 0, Value =  "i" },
                        new Edit<string>(){Action = 2, Value =  "s", From = "k" },
                    }
            };
        }

        public static IEnumerable<object[]> Integers()
        {
            yield return new object[]{
                    new int[]{ 1,2,3 },
                    new int[]{ 2,3,6},
                    new EditScript<int>(0){
                        new Edit<int>(){Action = 1, Value =  6 },
                        new Edit<int>(){Action =  0, Value =  3 },
                        new Edit<int>(){Action =  0, Value =  2 },
                        new Edit<int>(){Action = -1, Value =  1 },
                    }
            };
        }
        public static IEnumerable<object[]> StringWithSurrogatePair()
        {
            yield return new object[]{
                    "xあz",
                    "x𩸽z",
                    new EditScript<string>(1){
                        new Edit<string>(){Action = 0, Value = "z" },
                        new Edit<string>(){Action =  2, Value =  "𩸽",From = "あ" },
                        new Edit<string>(){Action = 0, Value =  "x" },
                    }
            };
        }
        public static IEnumerable<object[]> Nulls()
        {
            yield return new object[]{
                null,
                new string[] {"null" },
            };
            yield return new object[]{
                new string[] {"null" },
                null,
            };
        }

        public static IEnumerable<object[]> Show()
        {
            yield return new object[]{
                "",
                new EditScript<string>(0)
                {

                }
            };
            yield return new object[]{
                "+" + System.Environment.NewLine,
                new EditScript<string>(0)
                {
                    new Edit<string>(){Action = 1, Value = System.Environment.NewLine},

                }
            };
            yield return new object[]{
                "-w" + System.Environment.NewLine+
                "-z" + System.Environment.NewLine+
                "+y" + System.Environment.NewLine+
                "+x" ,
                new EditScript<string>(1)
                {
                    new Edit<string>(){Action = 1, Value = "x"},
                    new Edit<string>(){Action = 0, Value = "0"},
                    new Edit<string>(){Action = 2, Value = "y", From = "z"},
                    new Edit<string>(){Action = -1, Value = "w",},
                }
            };

        }

        public static IEnumerable<object[]> ContainsNull()
        {
            yield return new object[]{
                new string[] {"notnull",null,"notnull" },
                new string[] {"notnull","notnull" },
                new EditScript<string>(0){
                    new Edit<string>(){Action =  0, Value = "notnull" },
                    new Edit<string>(){Action = -1, Value = null },
                    new Edit<string>(){Action =  0, Value = "notnull" },
                }
            };
            yield return new object[]{
                new string[] {"notnull","notnull" },
                new string[] {"notnull",null,"notnull" },
                new EditScript<string>(0){
                    new Edit<string>(){Action =  0, Value = "notnull" },
                    new Edit<string>(){Action = 1, Value = null },
                    new Edit<string>(){Action =  0, Value = "notnull" },
                }
            };
            yield return new object[]{
                new string[] { null },
                new string[] { null },
                new EditScript<string>(0){
                    new Edit<string>(){Action =  0, Value = null },
                }
            };

        }

        public static IEnumerable<object[]> Diff3()
        {
            yield return new object[]{
                "\n1",
                "\n1\n2",
                "\n1\n2\n3",
                Push(0,
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 0, Value = "" } },
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 0, Value = "1" } },
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 1, Value = "2" } },
                    new Edit<IEdit<string>>(){ Action = 1, Value = new Edit<string>(){ Action = 1, Value = "3" } }
                    )
            };
            yield return new object[]{
                "\nclass A\n{\n    A(){}\n}\n",
                "\nclass A\n{\n    B(){}\n}\n",
                "\nclass A\n{\n    A(){}\n    B(){}\n}\n",
                Push(1,
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 0, Value = "" } },
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 0, Value = "class A" } },
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 0, Value = "{" } },
                    new Edit<IEdit<string>>(){
                        Action = 2,
                        From = new Edit<string>(){ Action = 2,  From = "    A(){}", Value = "    B(){}" },
                        Value = new Edit<string>(){ Action = 0, From = null , Value = "    A(){}", },
                    },
                    new Edit<IEdit<string>>(){
                        Action = 1,
                        Value = new Edit<string>(){ Action = 1, From = null , Value = "    B(){}", },
                    },
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 0, Value = "}" } },
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 0, Value = "" } }
                    )
            };

        }

        public static IEnumerable<object[]> StringDiff3()
        {
            yield return new object[]{
                "123",
                "123",
                "123",
                Push(0,
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 0, Value = "1" } },
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 0, Value = "2" } },
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 0, Value = "3" } })
            };
            yield return new object[]{
                "124",
                "1234",
                "12345",
                Push(0,
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 0, Value = "1" } },
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 0, Value = "2" } },
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 1, Value = "3" } },
                    new Edit<IEdit<string>>(){ Action = 0, Value = new Edit<string>(){ Action = 0, Value = "4" } },
                    new Edit<IEdit<string>>(){ Action = 1, Value = new Edit<string>(){ Action = 1, Value = "5" } })
            };
        }

        private static EditScript<T> Push<T>(int y, params Edit<T>[] x) where T : IEquatable<T>
        {
            var e = new EditScript<T>(y);
            foreach (var item in x.Reverse())
            {
                e.Add(item);
            }
            return e;
        }
    }

    public static class BenchmarkInput
    {
        public static IEnumerable<object[]> x500()
        {
            yield return new object[]{
                    Enumerable.Range(0,500).ToArray(),
                    Enumerable.Range(0,500).ToArray(),
                    Enumerable
                    .Range(0,500)
                    .Select(i =>new Edit<int>(){Action = 0, Value = i })
                    .To()
            };
        }
        public static IEnumerable<object[]> x1000()
        {
            yield return new object[]{
                    Enumerable.Range(0,1000).ToArray(),
                    Enumerable.Range(0,1000).ToArray(),
                    Enumerable
                    .Range(0,1000)
                    .Select(i =>new Edit<int>(){Action = 0, Value = i })
                    .To()
            };
        }
        public static IEnumerable<object[]> x2000()
        {
            yield return new object[]{
                    Enumerable.Range(0,2000).ToArray(),
                    Enumerable.Range(0,2000).ToArray(),
                    Enumerable
                    .Range(0,2000)
                    .Select(i =>new Edit<int>(){Action = 0, Value = i })
                    .To()
            };
        }

        private static EditScript<int> To(this IEnumerable<Edit<int>> from)
        {
            var to = new EditScript<int>(0);
            foreach (var item in from.Reverse())
            {
                to.Push(item);
            }
            return to;
        }
    }

}