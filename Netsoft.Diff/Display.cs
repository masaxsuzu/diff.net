using System;
using System.Linq;
using System.Text;

namespace Netsoft.Diff
{
    public static class Display
    {
        public static string Show<T>(this IEditScript<T> ses) where T : IEquatable<T>
        {
            return new StringBuilder()
                .AppendJoin(System.Environment.NewLine,
                    ses.Where(e => e.Action != 0)
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
                })).ToString();
        }
    }
}
