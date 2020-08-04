using System;
using System.Linq;
using System.Text;

namespace Netsoft.Diff
{
    public static class Display
    {
        public static string Show<T>(this IEditScript<T> ses) where T : IEquatable<T>
        {
            return ses.Where(e => e.Action != 0)
                .Select(e =>
                {
                    switch (e.Action)
                    {
                        case -1:
                            return $"-{e.Value}";
                        case 1:
                            return $"+{e.Value}";
                        case 2:
                            return
                            $"-{e.From}{System.Environment.NewLine}" +
                            $"+{e.Value}";
                        default:
                            return "";
                    }
                }).Aggregate((0, new StringBuilder()), (sb, s) => {
                    if (sb.Item1 == 0) {
                        return (sb.Item1+1, sb.Item2.Append(s));
                    } else {
                        return (sb.Item1+1, sb.Item2.Append(System.Environment.NewLine).Append(s));
                    }
                }).Item2.ToString();
        }
    }
}
