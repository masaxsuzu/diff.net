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
        public short Action { get; }
        T Value { get; }
    }
    internal class EditScript<T> : Stack<IEdit<T>>, IEditScript<T> where T : IEquatable<T>
    {
        public EditScript()
        {
        }

        public int Distance { get; private set; }
        public void Add(Edit<T> item)
        {
            Push(item);
            if (item?.Action != 0)
            {
                Distance++;
            }
        }
    }
    internal class Edit<T> : IEdit<T> where T : IEquatable<T>
    {
        public short Action { get; set; }
        public T Value { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            _ = Action == 0
                ? sb.Append("")
                : Action > 0
                ? sb.Append("+")
                : sb.Append("-");
            return sb.Append(Value).ToString();
        }
    }

}
