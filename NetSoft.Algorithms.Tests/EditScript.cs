using System.Collections.Generic;

namespace NetSoft.Algorithms.Tests
{
    public class TestInput
    {
        public static IEnumerable<object[]> Strings()
        {
            yield return new object[]{
                    new string[]{"h", "e" },
                    new string[]{"s","h", "e" },
                    new EditScript<string>(){
                        new Edit<string>(){Change = 1, Value = "s" },
                        new Edit<string>(){Change = 0, Value = "h" },
                        new Edit<string>(){Change = 0, Value = "e" },
                    }
                };
            yield return new object[]{
                    new string[]{"s","h", "e" },
                    new string[]{   "h", "e" },
                    new EditScript<string>(){
                        new Edit<string>(){Change = -1, Value =  "s" },
                        new Edit<string>(){Change = 0, Value =   "h" },
                        new Edit<string>(){Change = 0, Value =   "e" },
                    }
                };
            yield return new object[]{
                    new string[]{      "x=", "1*", "3", ";"},
                    new string[]{"let","x=", "1*",       ";"},
                    new EditScript<string>(){
                        new Edit<string>(){Change = 1, Value =  "let" },
                        new Edit<string>(){Change = 0, Value =  "x=" },
                        new Edit<string>(){Change = 0, Value =  "1*" },
                        new Edit<string>(){Change = -1, Value =  "3" },
                        new Edit<string>(){Change = 0, Value =  ";" },
                    }
                };
            yield return new object[]{
                    new string[]{"a","b", "c" },
                    new string[]{"a","x","c","d" },

                    new EditScript<string>(){
                        new Edit<string>(){Change = 0, Value =  "a" },
                        new Edit<string>(){Change = -1, Value =  "b" },
                        new Edit<string>(){Change = 1, Value =  "x" },
                        new Edit<string>(){Change = 0, Value =  "c" },
                        new Edit<string>(){Change = 1, Value =  "d" },
                    }
                };
            yield return new object[]{
                    new string[]{"k","i","t","t","e","n" },
                    new string[]{"s","i","t","t","i","n","g" },

                    new EditScript<string>(){
                        new Edit<string>(){Change = -1, Value =  "k" },
                        new Edit<string>(){Change = 1, Value =  "s" },
                        new Edit<string>(){Change = 0, Value =  "i" },
                        new Edit<string>(){Change = 0, Value =  "t" },
                        new Edit<string>(){Change = 0, Value =  "t" },
                        new Edit<string>(){Change = -1, Value =  "e" },
                        new Edit<string>(){Change = 1, Value =  "i" },
                        new Edit<string>(){Change = 0, Value =  "n" },
                        new Edit<string>(){Change = 1, Value =  "g" },
                    }
            };
        }

        public static IEnumerable<object[]> Integers()
        {
            yield return new object[]{
                    new int[]{ 1,2,3 },
                    new int[]{ 2,3,6},
                    new EditScript<int>(){
                        new Edit<int>(){Change = -1, Value =  1 },
                        new Edit<int>(){Change =  0, Value =  2 },
                        new Edit<int>(){Change =  0, Value =  3 },
                        new Edit<int>(){Change = 1, Value =  6 },
                    }
            };
        }
    }
}
