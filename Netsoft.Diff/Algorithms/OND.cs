using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Netsoft.Diff.Tests")]
namespace Netsoft.Diff.Algorithms
{
    internal static class OND
    {
        public static ChangeCollection<T> Diff<T>(T[] x, T[] y) where T : IEquatable<T>
        {
            int m = x.Length;
            int n = y.Length;
            int[] V = new int[m + n + 1];

            int offset = m;

            var path = new List<(int, int)> { };
            for (int D = 0; D <= m + n; D++)
            {
                int min = D <= m ? -D : D - (2 * m);
                int max = D <= n ? D : -D + (2 * n);
                for (int k = max; k >= min; k -= 2)
                {
                    int i = D == 0 ? 0
                          : k == -D ? V[offset + k + 1] + 1
                          : k == D ? V[offset + k - 1]
                          : Math.Max(V[offset + k + 1] + 1, V[offset + k - 1]);

                    path.Add((i, i + k));

                    while (i < m && i + k < n && x[i].NullSafeEquals(y[i + k]))
                    {
                        i += 1;
                        path.Add((i, i + k));
                    }
                    if (k == n - m && i == m)
                    {
                        return Backtrack<T>(new ChangeCollection<T>(0), path.ToArray(), x, y);
                    }
                    V[offset + k] = i;
                }
            }

            throw new NotImplementedException("[Unreachable code] Please kindly submit this bug to https://github.com/masaxsuzu/diff.net/issues.");
        }
        private static ChangeCollection<T> Backtrack<T>(ChangeCollection<T> ses, (int, int)[] path, T[] x, T[] y) where T : IEquatable<T>
        {
            int i = path.Length - 1;
            int k = 1;
            while (i > 0)
            {
                var p = path[i];
                var q = path[i - k];

                if (p.Item1 - q.Item1 == 1 && p.Item2 - q.Item2 == 1)
                {
                    ses = AppendRangeAtLast<T>(new Change<T>() { Action = 0, Value = x[q.Item1] }, ses, 0);
                }
                else if (p.Item2 - q.Item2 == 1 && p.Item1 == q.Item1)
                {
                    ses = AppendRangeAtLast<T>(new Change<T>() { Action = 1, Value = y[q.Item2] }, ses, 1);

                }
                else if (p.Item1 - q.Item1 == 1 && p.Item2 == q.Item2)
                {
                    if (ses._TryPeek(out var added) && added.Action == 1)
                    {
                        added = ses.Pop();
                        var replaced = new Change<T>() { Action = 2, From = x[q.Item1], Value = added.Value };
                        ses = AppendRangeAtLast<T>(replaced, ses, 1);
                    }
                    else
                    {
                        ses = AppendRangeAtLast<T>(new Change<T>() { Action = -1, Value = x[q.Item1] }, ses, 1);
                    }
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
        private static ChangeCollection<T> AppendRangeAtLast<T>(Change<T> c, ChangeCollection<T> r, int distance) where T : IEquatable<T>
        {
            r.Add(c, distance);
            return r;
        }

        private static bool _TryPeek<T>(this ChangeCollection<T> ses, out IChange<T> x) where T : IEquatable<T>
        {
            x = default;
            if(ses.Count == 0) {
                return false;
            }

            x = ses.Peek();
            return true;
        }
    }
}
