using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("NetSoft.Frameworks.Tests")]
namespace NetSoft.Frameworks
{
    public interface IEditScript<T> : IReadOnlyCollection<IEdit<T>> where T : IEquatable<T>
    {
        int Distance { get; }
    }

    public interface IEdit<T> where T : IEquatable<T>
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
