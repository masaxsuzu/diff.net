using Netsoft.Diff.Algorithms;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Netsoft.Diff
{
    public static class DiffExtension
    {
        public static IEditScript<T> Diff<T>(this T[] x, T[] y) where T : IEquatable<T>
        {
            if (x == null || y == null)
            {
                throw new ArgumentNullException();
            }
            return OND.Diff(x, y);
        }
        public static IEditScript<string> Diff(this string x, string y)
        {
            var a = StringInfo.GetTextElementEnumerator(x);
            var b = StringInfo.GetTextElementEnumerator(y);

            return OND.Diff<string>(a.ToStrings().ToArray(), b.ToStrings().ToArray());
        }

        public static IEditScript<IEdit<T>> Diff3<T>(this T[] a, T[] b, T[] c) where T : IEquatable<T>
        {
            var ab = a.Diff(b);
            var ac = a.Diff(c);

            return ab.ToArray().Diff(ac.ToArray());
        }
        public static IEditScript<IEdit<string>> Diff3(this string a, string b, string c)
        {
            var ab = a.Diff(b);
            var ac = a.Diff(c);

            return ab.ToArray().Diff(ac.ToArray());
        }
        private static IEnumerable<string> ToStrings(this TextElementEnumerator enumerator)
        {
            while (true)
            {
                if (!enumerator.MoveNext())
                {
                    break;
                }
                yield return enumerator.GetTextElement();
            }
        }
    }
}
