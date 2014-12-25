
namespace Anycmd.Query
{
    using System.Data.SqlClient;

    public class SqlFilter
    {
        public static readonly SqlFilter Empty = new SqlFilter(string.Empty, null);

        public SqlFilter(string filterString, SqlParameter[] parameters)
        {
            this.FilterString = filterString;
            this.Parameters = parameters;
        }

        public string FilterString { get; private set; }

        public SqlParameter[] Parameters { get; private set; }
    }
}
