using System;
using System.Collections.Generic;
using System.Text;

namespace NetSoft.Frameworks
{
    public class EditScript<T> : Stack<Edit<T>> where T : IEquatable<T>
    {
        public void Add(Edit<T> item)
        {
            Push(item);
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
