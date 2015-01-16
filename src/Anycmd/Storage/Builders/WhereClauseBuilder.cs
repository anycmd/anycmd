
namespace Anycmd.Storage.Builders
{
    using Anycmd.Properties;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using Util;

    /// <summary>
    /// 表示所有where子句建造器的基类。
    /// </summary>
    /// <typeparam name="TDataObject">对象类型，它通常被映射为关系数据库的表。</typeparam>
    public abstract class WhereClauseBuilder<TDataObject> : ExpressionVisitor, IWhereClauseBuilder<TDataObject>
        where TDataObject : class, new()
    {
        #region Private Fields
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly Dictionary<string, object> _parameterValues = new Dictionary<string, object>();
        private readonly IStorageMappingResolver _mappingResolver = null;
        private bool _startsWith = false;
        private bool _endsWith = false;
        private bool _contains = false;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个 <c>WhereClauseBuilderBase&lt;T&gt;</c> 类型的对象。
        /// </summary>
        /// <param name="mappingResolver">The <c>anycmd.Storage.IStorageMappingResolver</c>
        /// instance which will be used for generating the mapped field names.</param>
        protected WhereClauseBuilder(IStorageMappingResolver mappingResolver)
        {
            this._mappingResolver = mappingResolver;
        }
        #endregion

        #region Private Methods
        private void Out(string s)
        {
            _sb.Append(s);
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// Gets a <c>System.String</c> value which represents the AND operation in the WHERE clause.
        /// </summary>
        protected virtual string And
        {
            get { return "AND"; }
        }
        /// <summary>
        /// Gets a <c>System.String</c> value which represents the OR operation in the WHERE clause.
        /// </summary>
        protected virtual string Or
        {
            get { return "OR"; }
        }
        /// <summary>
        /// Gets a <c>System.String</c> value which represents the EQUAL operation in the WHERE clause.
        /// </summary>
        protected virtual string Equal
        {
            get { return "="; }
        }
        /// <summary>
        /// Gets a <c>System.String</c> value which represents the NOT operation in the WHERE clause.
        /// </summary>
        protected virtual string Not
        {
            get { return "NOT"; }
        }
        /// <summary>
        /// Gets a <c>System.String</c> value which represents the NOT EQUAL operation in the WHERE clause.
        /// </summary>
        protected virtual string NotEqual
        {
            get { return "<>"; }
        }
        /// <summary>
        /// Gets a <c>System.String</c> value which represents the LIKE operation in the WHERE clause.
        /// </summary>
        protected virtual string Like
        {
            get { return "LIKE"; }
        }
        /// <summary>
        /// Gets a <c>System.Char</c> value which represents the place-holder for the wildcard in the LIKE operation.
        /// </summary>
        protected virtual char LikeSymbol
        {
            get { return '%'; }
        }
        /// <summary>
        /// Gets a <c>System.Char</c> value which represents the leading character to be used by the
        /// database parameter.
        /// </summary>
        protected internal abstract char ParameterChar { get; }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.BinaryExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            string str;
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    str = "+";
                    break;
                case ExpressionType.AddChecked:
                    str = "+";
                    break;
                case ExpressionType.AndAlso:
                    str = this.And;
                    break;
                case ExpressionType.Divide:
                    str = "/";
                    break;
                case ExpressionType.Equal:
                    str = this.Equal;
                    break;
                case ExpressionType.GreaterThan:
                    str = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    str = ">=";
                    break;
                case ExpressionType.LessThan:
                    str = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    str = "<=";
                    break;
                case ExpressionType.Modulo:
                    str = "%";
                    break;
                case ExpressionType.Multiply:
                    str = "*";
                    break;
                case ExpressionType.MultiplyChecked:
                    str = "*";
                    break;
                case ExpressionType.Not:
                    str = this.Not;
                    break;
                case ExpressionType.NotEqual:
                    str = this.NotEqual;
                    break;
                case ExpressionType.OrElse:
                    str = this.Or;
                    break;
                case ExpressionType.Subtract:
                    str = "-";
                    break;
                case ExpressionType.SubtractChecked:
                    str = "-";
                    break;
                default:
                    throw new NotSupportedException(string.Format(Resources.EX_EXPRESSION_NODE_TYPE_NOT_SUPPORT, node.NodeType));
            }

            Out("(");
            Visit(node.Left);
            Out(" ");
            Out(str);
            Out(" ");
            Visit(node.Right);
            Out(")");
            return node;
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.MemberExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.DeclaringType == typeof(TDataObject) ||
                (node.Member.DeclaringType != null && typeof(TDataObject).IsSubclassOf(node.Member.DeclaringType)))
            {
                var mappedFieldName = _mappingResolver.ResolveFieldName<TDataObject>(node.Member.Name);
                Out(mappedFieldName);
            }
            else
            {
                if (node.Member is FieldInfo)
                {
                    var ce = node.Expression as ConstantExpression;
                    var fi = node.Member as FieldInfo;
                    var fieldValue = fi.GetValue(ce.Value);
                    Expression constantExpr = Expression.Constant(fieldValue);
                    Visit(constantExpr);
                }
                else
                    throw new NotSupportedException(string.Format(Resources.EX_MEMBER_TYPE_NOT_SUPPORT, node.Member.GetType().FullName));
            }
            return node;
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.ConstantExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            var paramName = string.Format("{0}{1}", ParameterChar, ReflectionHelper.GetUniqueIdentifier(5));
            Out(paramName);
            if (_parameterValues.ContainsKey(paramName)) return node;
            object v = null;
            if (_startsWith && node.Value is string)
            {
                _startsWith = false;
                v = node.Value.ToString() + LikeSymbol;
            }
            else if (_endsWith && node.Value is string)
            {
                _endsWith = false;
                v = LikeSymbol + node.Value.ToString();
            }
            else if (_contains && node.Value is string)
            {
                _contains = false;
                v = LikeSymbol + node.Value.ToString() + LikeSymbol;
            }
            else
            {
                v = node.Value;
            }
            _parameterValues.Add(paramName, v);
            return node;
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.MethodCallExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Out("(");
            Visit(node.Object);
            if (node.Arguments == null || node.Arguments.Count != 1)
                throw new NotSupportedException(Resources.EX_INVALID_METHOD_CALL_ARGUMENT_NUMBER);
            Expression expr = node.Arguments[0];
            switch (node.Method.Name)
            {
                case "StartsWith":
                    _startsWith = true;
                    Out(" ");
                    Out(Like);
                    Out(" ");
                    break;
                case "EndsWith":
                    _endsWith = true;
                    Out(" ");
                    Out(Like);
                    Out(" ");
                    break;
                case "Equals":
                    Out(" ");
                    Out(Equal);
                    Out(" ");
                    break;
                case "Contains":
                    _contains = true;
                    Out(" ");
                    Out(Like);
                    Out(" ");
                    break;
                default:
                    throw new NotSupportedException(string.Format(Resources.EX_METHOD_NOT_SUPPORT, node.Method.Name));
            }
            if (expr is ConstantExpression || expr is MemberExpression)
                Visit(expr);
            else
                throw new NotSupportedException(string.Format(Resources.EX_METHOD_CALL_ARGUMENT_TYPE_NOT_SUPPORT, expr.GetType().ToString()));
            Out(")");
            return node;
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.BlockExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitBlock(BlockExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.CatchBlock"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.ConditionalExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitConditional(ConditionalExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.DebugInfoExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.DefaultExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitDefault(DefaultExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.DynamicExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitDynamic(System.Linq.Expressions.DynamicExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.ElementInit"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override ElementInit VisitElementInit(ElementInit node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.GotoExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitGoto(GotoExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.Expression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitExtension(Expression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.IndexExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitIndex(IndexExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.InvocationExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitInvocation(InvocationExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.LabelExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitLabel(LabelExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.LabelTarget"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.Expression&lt;T&gt;"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.ListInitExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitListInit(ListInitExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.LoopExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitLoop(LoopExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.MemberAssignment"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.MemberBinding"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.MemberInitExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.MemberListBinding"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.MemberMemberBinding"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.NewExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitNew(NewExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.NewArrayExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.ParameterExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.RuntimeVariablesExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.SwitchExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitSwitch(SwitchExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.SwitchCase"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.TryExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitTry(TryExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.TypeBinaryExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        /// <summary>
        /// Visits the children of <see cref="System.Linq.Expressions.UnaryExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.</returns>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            throw new NotSupportedException(string.Format(Resources.EX_PROCESS_NODE_NOT_SUPPORT, node.GetType().Name));
        }
        #endregion

        #region IWhereClauseBuilder<T> Members
        /// <summary>
        /// Builds the WHERE clause from the given expression object.
        /// </summary>
        /// <param name="expression">The expression object.</param>
        /// <returns>The <c>anycmd.Storage.Builders.WhereClauseBuildResult</c> instance
        /// which contains the build result.</returns>
        public WhereClauseBuildResult BuildWhereClause(Expression<Func<TDataObject, bool>> expression)
        {
            this._sb.Clear();
            this._parameterValues.Clear();
            this.Visit(expression.Body);
            var result = new WhereClauseBuildResult
            {
                ParameterValues = _parameterValues,
                WhereClause = _sb.ToString()
            };
            return result;
        }
        #endregion
    }
}
