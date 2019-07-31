using System;

namespace Netsoft.Diff.Algorithms
{
    internal static class NullSafeHelper
    {
        public static bool NullSafeEquals<T>(this T x, T y) where T : IEquatable<T>
        {
            if (x != null)
            {
                return x.Equals(y);
            }
            if (y != null)
            {
                return y.Equals(x);
            }
            return true;
        }
    }
}
