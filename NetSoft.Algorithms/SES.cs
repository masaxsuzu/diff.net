using System;
using System.Collections.Generic;
using System.Text;

namespace NetSoft.Algorithms
{
    public static class SES<T> where T : IEquatable<T>
    {
        public static Difference<T> Diff(T[] x, T[] y)
        {
            int m = x.Length;
            int n = y.Length;
            int[][] editGraph = InitEditGraph(m, n);

            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    editGraph[i][j] = x[i - 1].Equals(y[j - 1])
                        ? editGraph[i - 1][j - 1]
                        : Math.Min(editGraph[i][j - 1] + 1, editGraph[i - 1][j] + 1);
                }
            }

#if DEBUG
            DumpEditGraph(editGraph);
#endif

            return Backtrack(new Difference<T>(), editGraph, x, y, m, n);
        }

        private static Difference<T> Backtrack(Difference<T> ses, int[][] editGraph, T[] x, T[] y, int i, int j)
        {
            if (i == 0 && j == 0)
            {
                return ses;
            }

            if (i == 0)
            {
                return TraceAdd(ses, editGraph, x, y, i, j);
            }

            if (j == 0)
            {
                return TraceDelete(ses, editGraph, x, y, i, j);
            }

            return Trace(ses, editGraph, x, y, i, j);
        }

        private static Difference<T> Trace(Difference<T> ses, int[][] editGraph, T[] x, T[] y, int i, int j)
        {
            int now = editGraph[i][j];
            int unchage = editGraph[i - 1][j - 1];
            int add = editGraph[i][j - 1];
            int delete = editGraph[i - 1][j];

            if (unchage < add && unchage < delete && now == unchage)
            {
                return TraceUnchange(ses, editGraph, x, y, i, j);
            }
            if (add <= delete)
            {
                return TraceAdd(ses, editGraph, x, y, i, j);
            }

            return TraceDelete(ses, editGraph, x, y, i, j);
        }

        #region Unchange/Add/Delete
        private static Difference<T> TraceUnchange(Difference<T> ses, int[][] editGraph, T[] x, T[] y, int i, int j)
        {
            return Backtrack(AppendRangeAtLast(new Edit<T>() { Change = 0, Value = x[i - 1] }, ses),
                                editGraph, x, y, i - 1, j - 1);
        }

        private static Difference<T> TraceDelete(Difference<T> ses, int[][] editGraph, T[] x, T[] y, int i, int j)
        {
            return Backtrack(AppendRangeAtLast(new Edit<T>() { Change = -1, Value = x[i - 1] }, ses),
                                editGraph, x, y, i - 1, j);
        }

        private static Difference<T> TraceAdd(Difference<T> ses, int[][] editGraph, T[] x, T[] y, int i, int j)
        {
            return Backtrack(AppendRangeAtLast(new Edit<T>() { Change = 1, Value = y[j - 1] }, ses),
                                editGraph, x, y, i, j - 1);
        }
        #endregion

        private static Difference<T> AppendRangeAtLast(Edit<T> c, Difference<T> r)
        {
            var x = new Difference<T>()
            {
                c
            };
            x.AddRange(r);
            return x;
        }

        private static int[][] InitEditGraph(int m, int n)
        {
            int[][] editGraph = new int[m + 1][];

            for (int i = 0; i <= m; i++)
            {
                editGraph[i] = new int[n + 1];
                editGraph[i][0] = i;
            }

            for (int j = 0; j <= n; j++)
            {
                editGraph[0][j] = j;
            }
            return editGraph;
        }

        [System.Diagnostics.Conditional("DEBUG")]
        private static void DumpEditGraph(int[][] g)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < g.Length; i++)
            {
                for (int j = 0; j < g[0].Length; j++)
                {
                    _ = sb.Append($"{g[i][j]}\t");
                }
                _ = sb.AppendLine();
            }
            string x = sb.ToString();
        }
    }
    public class Difference<T> : List<Edit<T>> where T : IEquatable<T>
    {
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var item in this)
            {
                _ = sb.Append($"{item}\n");
            }
            return sb.ToString();
        }
    }
    public class Edit<T> : IComparable<Edit<T>> where T : IEquatable<T>
    {
        public short Change { get; set; }
        public T Value { get; set; }

        public int CompareTo(Edit<T> other)
        {
            if (Change != other.Change)
            {
                return Change - other.Change;
            }

            return Value.Equals(other.Value) ? 0 : 1;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            _ = Change == 0
                ? sb.Append("")
                : Change > 0
                ? sb.Append("+")
                : sb.Append("-");
            return sb.Append(Value).ToString();
        }
    }
}

