
namespace Anycmd.Query.UnifiedQueries
{
    public partial class UnaryLogicalOperation
    {
        public override string ToString()
        {
            return string.Format("({0} {1})", this.Operator, this.Item);
        }
    }
}
