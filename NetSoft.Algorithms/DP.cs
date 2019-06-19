using System;
using System.Collections.Generic;
using System.Text;
namespace NetSoft.Algorithms
{
    public static class DP
    {
        /// <summary>
        /// Diff computes SES(Shortest Edit Script) between two gitven sequences by DP(Dynamic Programming).
        /// </summary>
        public static EditScript<T> Diff<T>(this T[] x, T[] y) where T : IEquatable<T>
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

            return Backtrack(new EditScript<T>(), editGraph, x, y, m, n);
        }

        private static EditScript<T> Backtrack<T>(EditScript<T> ses, int[][] editGraph, T[] x, T[] y, int i, int j)
            where T : IEquatable<T>
        {
            if (i == 0 && j == 0)
            {
                return ses;
            }

            if (i == 0)
            {
                return TrackAdd(ses, editGraph, x, y, i, j);
            }

            if (j == 0)
            {
                return TrackDelete(ses, editGraph, x, y, i, j);
            }

            return Track(ses, editGraph, x, y, i, j);
        }

        private static EditScript<T> Track<T>(EditScript<T> ses, int[][] editGraph, T[] x, T[] y, int i, int j)
            where T : IEquatable<T>
        {
            int now = editGraph[i][j];
            int unchage = editGraph[i - 1][j - 1];
            int add = editGraph[i][j - 1];
            int delete = editGraph[i - 1][j];

            if (unchage < add && unchage < delete && now == unchage)
            {
                return TrackUnchange(ses, editGraph, x, y, i, j);
            }
            if (add <= delete)
            {
                return TrackAdd(ses, editGraph, x, y, i, j);
            }

            return TrackDelete(ses, editGraph, x, y, i, j);
        }

        #region Unchange/Add/Delete
        private static EditScript<T> TrackUnchange<T>(EditScript<T> ses, int[][] editGraph, T[] x, T[] y, int i, int j)
            where T : IEquatable<T>
        {
            return Backtrack(AppendRangeAtLast(new Edit<T>() { Change = 0, Value = x[i - 1] }, ses),
                                editGraph, x, y, i - 1, j - 1);
        }

        private static EditScript<T> TrackDelete<T>(EditScript<T> ses, int[][] editGraph, T[] x, T[] y, int i, int j)
            where T : IEquatable<T>
        {
            return Backtrack(AppendRangeAtLast(new Edit<T>() { Change = -1, Value = x[i - 1] }, ses),
                                editGraph, x, y, i - 1, j);
        }

        private static EditScript<T> TrackAdd<T>(EditScript<T> ses, int[][] editGraph, T[] x, T[] y, int i, int j)
            where T : IEquatable<T>
        {
            return Backtrack(AppendRangeAtLast(new Edit<T>() { Change = 1, Value = y[j - 1] }, ses),
                                editGraph, x, y, i, j - 1);
        }
        #endregion

        private static EditScript<T> AppendRangeAtLast<T>(Edit<T> c, EditScript<T> r) where T : IEquatable<T>
        {
            r.Push(c);
            return r;
        }

        public static int[][] InitEditGraph(int m, int n)
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
        public static void DumpEditGraph(int[][] g)
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
    public class EditScript<T> : Stack<Edit<T>> where T : IEquatable<T>
    {
        public void Add(Edit<T> item)
        {
            Push(item);
        }
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
    public class Edit<T> where T : IEquatable<T>
    {
        public short Change { get; set; }
        public T Value { get; set; }

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

