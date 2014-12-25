using System.Data.Entity.Core.Objects;

namespace Anycmd.Ef
{
    public class ObjectFilter
    {
        public static readonly ObjectFilter Empty = new ObjectFilter(string.Empty, null);

        public ObjectFilter(string filterString, ObjectParameter[] parameters)
        {
            this.FilterString = filterString;
            this.Parameters = parameters;
        }

        public string FilterString { get; private set; }

        public ObjectParameter[] Parameters { get; private set; }
    }
}
