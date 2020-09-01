using Netsoft.Diff.Algorithms;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Netsoft.Diff
{
    public static class Differences
    {
        public static IChangeCollection<T> Between<T>(T[] x, T[] y) where T : IEquatable<T>
        {
            if (x == null || y == null)
            {
                throw new ArgumentNullException();
            }
            return OND.Diff(x, y);
        }
        public static IChangeCollection<string> Between(string x, string y)
        {
            var a = StringInfo.GetTextElementEnumerator(x);
            var b = StringInfo.GetTextElementEnumerator(y);

            return OND.Diff<string>(a.ToStrings().ToArray(), b.ToStrings().ToArray());
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
