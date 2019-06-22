using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NetSoft.Frameworks.Tests")]
namespace NetSoft.Frameworks.Algorithms
{
    internal static class ONP
    {
        public static EditScript<T> Diff<T>(T[] x, T[] y) where T : IEquatable<T>
        {
            if (x.Length > y.Length)
            {
                return Diff(y, x, true);
            }
            return Diff(x, y, false);
        }
        private static EditScript<T> Diff<T>(this T[] x, T[] y, bool reversed) where T : IEquatable<T>
        {
            int m = x.Length;
            int n = y.Length;
            int[] V = new int[m + n + 1];

            int offset = m;
            int delta = n - m;
            var path = new SortedSet<(int, int)> {
                (0,0),
                (m,n),
            };

            for (int p = 0; p <= m; p++)
            {
                for (int k = -p; k < delta; k++)
                {
                    int i = p == 0 ? k == 0 ? 0 : V[offset + k - 1]
                        : k == -p ? V[offset + k + 1] + 1
                        : Math.Max(V[offset + k + 1] + 1, V[offset + k - 1]);
                    _ = path.Add((i, i + k));
                    while (i < m && i + k < n && x[i].Equals(y[i + k]))
                    {
                        i += 1;
                        _ = path.Add((i, i + k));
                    }
                    V[offset + k] = i;
                }
                for (int k = delta + p; k > delta; k--)
                {
                    int i = k == delta + p ? V[offset + k - 1]
                        : Math.Max(V[offset + k + 1] + 1, V[offset + k - 1]);
                    _ = path.Add((i, i + k));
                    while (i < m && i + k < n && x[i].Equals(y[i + k]))
                    {
                        i += 1;
                        _ = path.Add((i, i + k));
                    }
                    V[offset + k] = i;
                }
                {
                    int k = delta;
                    int i = p == 0 ? k == 0 ? 0 : V[offset + k - 1]
                        : Math.Max(V[offset + k + 1] + 1, V[offset + k - 1]);
                    _ = path.Add((i, i + k));

                    while (i < m && i + k < n && x[i].Equals(y[i + k]))
                    {
                        i += 1;
                        _ = path.Add((i, i + k));
                    }
                    if (i == m)
                    {
                        while (i + k < n)
                        {
                            _ = path.Add((i, i + k));
                            k++;
                        }
                        return Backtrack<T>(new EditScript<T>(), path.ToArray(), x, y, reversed);
                    }
                    V[offset + k] = i;
                }
            }

            throw new NotImplementedException("Found a bug. Never reach here if the implementation is correct.");
        }
        private static EditScript<T> Backtrack<T>(EditScript<T> ses, (int, int)[] path, T[] x, T[] y, bool reversed) where T : IEquatable<T>
        {
            int i = path.Length - 1;
            int k = 1;

            short added = reversed ? (short)-1 : (short)1;
            short removed = reversed ? (short)1 : (short)-1;
            while (i > 0)
            {
                var p = path[i];
                var q = path[i - k];

                if (p.Item1 - q.Item1 == 1 && p.Item2 - q.Item2 == 1)
                {
                    ses = AppendRangeAtLast<T>(new Edit<T>() { Change = 0, Value = x[q.Item1] }, ses);
                }
                else if (p.Item2 - q.Item2 == 1 && p.Item1 == q.Item1)
                {
                    ses = AppendRangeAtLast<T>(new Edit<T>() { Change = added, Value = y[q.Item2] }, ses);
                }
                else if (p.Item1 - q.Item1 == 1 && p.Item2 == q.Item2)
                {
                    ses = AppendRangeAtLast<T>(new Edit<T>() { Change = removed, Value = x[q.Item1] }, ses);
                }


                else
                {
                    k++;
                    continue;
                }
                i -= k;
                k = 1;

            }
            return ses;
        }
        private static EditScript<T> AppendRangeAtLast<T>(Edit<T> c, EditScript<T> r) where T : IEquatable<T>
        {
            r.Push(c);
            return r;
        }
    }
}