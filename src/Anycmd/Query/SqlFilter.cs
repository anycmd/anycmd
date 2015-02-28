
namespace Anycmd.Query
{
    using System.Data.Common;

    public class SqlFilter
    {
        public static readonly SqlFilter Empty = new SqlFilter(string.Empty, null);

        public SqlFilter(string filterString, DbParameter[] parameters)
        {
            this.FilterString = filterString;
            this.Parameters = parameters;
        }

        public string FilterString { get; private set; }

        public DbParameter[] Parameters { get; private set; }
    }
}
