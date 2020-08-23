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
                    new ChangeCollection<string>(0){
                        new Change<string>(){Action = 0, Value = "e" },
                        new Change<string>(){Action = 0, Value = "h" },
                        new Change<string>(){Action = 1, Value = "s" },
                    }
                };
            yield return new object[]{
                    new string[]{"s","h", "e" },
                    new string[]{   "h", "e" },
                    new ChangeCollection<string>(0){
                        new Change<string>(){Action = 0, Value =   "e" },
                        new Change<string>(){Action = 0, Value =   "h" },
                        new Change<string>(){Action = -1, Value =  "s" },
                    }
                };
            yield return new object[]{
                    new string[]{      "x=", "1*", "3", ";"},
                    new string[]{"let","x=", "1*",       ";"},
                    new ChangeCollection<string>(0){
                        new Change<string>(){Action = 0, Value =  ";" },
                        new Change<string>(){Action = -1, Value =  "3" },
                        new Change<string>(){Action = 0, Value =  "1*" },
                        new Change<string>(){Action = 0, Value =  "x=" },
                        new Change<string>(){Action = 1, Value =  "let" },
                    }
                };
            yield return new object[]{
                    new string[]{"a","b", "c" },
                    new string[]{"a","x","c","d" },

                    new ChangeCollection<string>(1){
                        new Change<string>(){Action = 1, Value =  "d" },
                        new Change<string>(){Action = 0, Value =  "c" },
                        new Change<string>(){Action = 2, Value =  "x", From = "b" },
                        new Change<string>(){Action = 0, Value =  "a" },
                    }
                };
            yield return new object[]{
                    new string[]{"k","i","t","t","e","n" },
                    new string[]{"s","i","t","t","i","n","g" },

                    new ChangeCollection<string>(2){
                        new Change<string>(){Action = 1, Value =  "g" },
                        new Change<string>(){Action = 0, Value =  "n" },
                        new Change<string>(){Action = 2, Value =  "i", From = "e" },
                        new Change<string>(){Action = 0, Value =  "t" },
                        new Change<string>(){Action = 0, Value =  "t" },
                        new Change<string>(){Action = 0, Value =  "i" },
                        new Change<string>(){Action = 2, Value =  "s", From = "k" },
                    }
            };
        }

        public static IEnumerable<object[]> Integers()
        {
            yield return new object[]{
                    new int[]{ 1,2,3 },
                    new int[]{ 2,3,6},
                    new ChangeCollection<int>(0){
                        new Change<int>(){Action = 1, Value =  6 },
                        new Change<int>(){Action =  0, Value =  3 },
                        new Change<int>(){Action =  0, Value =  2 },
                        new Change<int>(){Action = -1, Value =  1 },
                    }
            };
        }
        public static IEnumerable<object[]> StringWithSurrogatePair()
        {
            yield return new object[]{
                    "xあz",
                    "x𩸽z",
                    new ChangeCollection<string>(1){
                        new Change<string>(){Action = 0, Value = "z" },
                        new Change<string>(){Action =  2, Value =  "𩸽",From = "あ" },
                        new Change<string>(){Action = 0, Value =  "x" },
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
                new ChangeCollection<string>(0)
                {

                }
            };
            yield return new object[]{
                "+" + System.Environment.NewLine,
                new ChangeCollection<string>(0)
                {
                    new Change<string>(){Action = 1, Value = System.Environment.NewLine},

                }
            };
            yield return new object[]{
                "-w" + System.Environment.NewLine+
                "-z" + System.Environment.NewLine+
                "+y" + System.Environment.NewLine+
                "+x" ,
                new ChangeCollection<string>(1)
                {
                    new Change<string>(){Action = 1, Value = "x"},
                    new Change<string>(){Action = 0, Value = "0"},
                    new Change<string>(){Action = 2, Value = "y", From = "z"},
                    new Change<string>(){Action = -1, Value = "w",},
                }
            };

        }

        public static IEnumerable<object[]> ContainsNull()
        {
            yield return new object[]{
                new string[] {"notnull",null,"notnull" },
                new string[] {"notnull","notnull" },
                new ChangeCollection<string>(0){
                    new Change<string>(){Action =  0, Value = "notnull" },
                    new Change<string>(){Action = -1, Value = null },
                    new Change<string>(){Action =  0, Value = "notnull" },
                }
            };
            yield return new object[]{
                new string[] {"notnull","notnull" },
                new string[] {"notnull",null,"notnull" },
                new ChangeCollection<string>(0){
                    new Change<string>(){Action =  0, Value = "notnull" },
                    new Change<string>(){Action = 1, Value = null },
                    new Change<string>(){Action =  0, Value = "notnull" },
                }
            };
            yield return new object[]{
                new string[] { null },
                new string[] { null },
                new ChangeCollection<string>(0){
                    new Change<string>(){Action =  0, Value = null },
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
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 0, Value = "" } },
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 0, Value = "1" } },
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 1, Value = "2" } },
                    new Change<IChange<string>>(){ Action = 1, Value = new Change<string>(){ Action = 1, Value = "3" } }
                    )
            };
            yield return new object[]{
                "\nclass A\n{\n    A(){}\n}\n",
                "\nclass A\n{\n    B(){}\n}\n",
                "\nclass A\n{\n    A(){}\n    B(){}\n}\n",
                Push(1,
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 0, Value = "" } },
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 0, Value = "class A" } },
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 0, Value = "{" } },
                    new Change<IChange<string>>(){
                        Action = 2,
                        From = new Change<string>(){ Action = 2,  From = "    A(){}", Value = "    B(){}" },
                        Value = new Change<string>(){ Action = 0, From = null , Value = "    A(){}", },
                    },
                    new Change<IChange<string>>(){
                        Action = 1,
                        Value = new Change<string>(){ Action = 1, From = null , Value = "    B(){}", },
                    },
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 0, Value = "}" } },
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 0, Value = "" } }
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
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 0, Value = "1" } },
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 0, Value = "2" } },
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 0, Value = "3" } })
            };
            yield return new object[]{
                "124",
                "1234",
                "12345",
                Push(0,
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 0, Value = "1" } },
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 0, Value = "2" } },
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 1, Value = "3" } },
                    new Change<IChange<string>>(){ Action = 0, Value = new Change<string>(){ Action = 0, Value = "4" } },
                    new Change<IChange<string>>(){ Action = 1, Value = new Change<string>(){ Action = 1, Value = "5" } })
            };
        }

        private static ChangeCollection<T> Push<T>(int y, params Change<T>[] x) where T : IEquatable<T>
        {
            var e = new ChangeCollection<T>(y);
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
                    .Select(i =>new Change<int>(){Action = 0, Value = i })
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
                    .Select(i =>new Change<int>(){Action = 0, Value = i })
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
                    .Select(i =>new Change<int>(){Action = 0, Value = i })
                    .To()
            };
        }

        private static ChangeCollection<int> To(this IEnumerable<Change<int>> from)
        {
            var to = new ChangeCollection<int>(0);
            foreach (var item in from.Reverse())
            {
                to.Push(item);
            }
            return to;
        }
    }

}