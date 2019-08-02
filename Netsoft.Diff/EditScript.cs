using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Netsoft.Diff.Tests")]
namespace Netsoft.Diff
{
    public interface IEditScript<T> : IReadOnlyCollection<IEdit<T>> where T : IEquatable<T>
    {
        int Distance { get; }
    }

    public interface IEdit<T> : IEquatable<IEdit<T>> where T : IEquatable<T>
    {
        short Action { get; }
        T From { get; }
        T Value { get; }
    }
    internal class EditScript<T> : Stack<IEdit<T>>, IEditScript<T> where T : IEquatable<T>
    {
        public EditScript(int init)
        {
            Distance = init;
        }

        public int Distance { get; private set; }
        internal void Add(Edit<T> item)
        {
            Add(item, 1);
        }
        public void Add(Edit<T> item, int distance)
        {
            Push(item);
            if (item?.Action != 0)
            {
                Distance += distance;
            }
        }
    }
    internal class Edit<T> : IEdit<T> where T : IEquatable<T>
    {
        public short Action { get; set; }
        public T Value { get; set; }
        public T From { get; set; }

        public bool Equals(IEdit<T> other)
        {
            if (other == null)
            {
                return false;
            }

            return Action == other.Action &&
                Equals(Value, other.Value) &&
                Equals(From, other.From);
        }

        private bool Equals(T x, T y)
        {
            if (x == null)
            {
                return y == null;
            }
            return x.Equals(y);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            switch (Action)
            {
                case 0:
                    _ = sb.Append("");
                    break;
                case 1:
                    _ = sb.Append("+");
                    break;
                case -1:
                    _ = sb.Append("-");
                    break;
                case 2:
                    _ = sb.Append(From);
                    _ = sb.Append("->");
                    break;

                default:
                    break;
            }
            return sb.Append(Value).ToString();
        }
    }

}
