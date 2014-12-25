
namespace Anycmd.Specifications
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Represents the parameter rebinder used for rebinding the parameters
    /// for the given expressions. 这是解决当使用Anycmd规约时在Entity Framework中遇到的表达式参数问题的解决方案的一部分。 
    /// 更多信息请参见 http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx.
    /// </summary>
    internal class ParameterRebinder : ExpressionVisitor
    {
        #region Private Fields
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;
        #endregion

        #region Ctor
        internal ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this._map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }
        #endregion

        #region Internal Static Methods
        internal static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }
        #endregion

        #region Protected Methods
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (_map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
        #endregion
    }
}
