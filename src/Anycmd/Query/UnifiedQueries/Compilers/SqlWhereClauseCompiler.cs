namespace Anycmd.Query.UnifiedQueries.Compilers
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 表示可以将查询规约编译为关系数据库的where子句的编译器。
    /// </summary>
    public class SqlWhereClauseCompiler : QuerySpecificationCompiler<string>
    {
        /// <summary>
        /// 参数值。
        /// </summary>
        private readonly Dictionary<string, object> _parameterValues = new Dictionary<string, object>();

        /// <summary>
        /// 是否使用参数化sql。
        /// </summary>
        private bool _useParameter;

        /// <summary>
        /// 初始化一个 <see cref="SqlWhereClauseCompiler"/> 类型的对象。
        /// </summary>
        public SqlWhereClauseCompiler()
        {
        }

        /// <summary>
        /// 初始化一个 <see cref="SqlWhereClauseCompiler"/> 类型的对象。
        /// </summary>
        /// <param name="useParameter">是否使用参数化sql。true表示使用，false表示不使用。</param>
        public SqlWhereClauseCompiler(bool useParameter)
        {
            this._useParameter = useParameter;
        }


        public IEnumerable<KeyValuePair<string, object>> ParameterValues
        {
            get
            {
                return this._parameterValues;
            }
        }

        public bool UseParameter
        {
            get
            {
                return this._useParameter;
            }

            set
            {
                this._useParameter = value;
            }
        }

        protected virtual char LikeSymbol
        {
            get
            {
                return '%';
            }
        }

        protected virtual char ParameterChar
        {
            get
            {
                return '@';
            }
        }

        protected virtual string GetExpressionPresentation(Expression expr)
        {
            var sb = new StringBuilder("(");
            sb.Append(expr.Name);
            switch (expr.Operator)
            {
                case RelationalOperators.Contains:
                case RelationalOperators.EndsWith:
                case RelationalOperators.StartsWith:
                    sb.Append(" LIKE ");
                    break;
                case RelationalOperators.EqualTo:
                    sb.Append(" = ");
                    break;
                case RelationalOperators.GreaterThan:
                    sb.Append(" > ");
                    break;
                case RelationalOperators.GreaterThanOrEqualTo:
                    sb.Append(" >= ");
                    break;
                case RelationalOperators.LessThan:
                    sb.Append(" < ");
                    break;
                case RelationalOperators.LessThanOrEqualTo:
                    sb.Append(" <= ");
                    break;
            }

            if (this._useParameter)
            {
                if (expr.Type == DataTypes.String)
                {
                    switch (expr.Operator)
                    {
                        case RelationalOperators.StartsWith:
                            expr.Value = string.Format("{0}{1}", expr.Value, this.LikeSymbol);
                            break;
                        case RelationalOperators.EndsWith:
                            expr.Value = string.Format("{0}{1}", this.LikeSymbol, expr.Value);
                            break;
                        default:
                            expr.Value = string.Format("{1}{0}{1}", expr.Value, this.LikeSymbol);
                            break;
                    }
                }

                var value = expr.GetValue();
                var parameterName = string.Format("{0}{1}", this.ParameterChar, Utils.GetUniqueIdentifier(6));
                this._parameterValues.Add(parameterName, value);
                sb.AppendFormat(parameterName);
            }
            else
            {
                switch (expr.Type)
                {
                    case DataTypes.Char:
                        sb.AppendFormat("'{0}'", expr.Value);
                        break;
                    case DataTypes.String:
                        switch (expr.Operator)
                        {
                            case RelationalOperators.StartsWith:
                                sb.AppendFormat("'{0}{1}'", expr.Value, this.LikeSymbol);
                                break;
                            case RelationalOperators.EndsWith:
                                sb.AppendFormat("'{0}{1}'", this.LikeSymbol, expr.Value);
                                break;
                            default:
                                sb.AppendFormat("'{1}{0}{1}'", expr.Value, this.LikeSymbol);
                                break;
                        }

                        break;
                    default:
                        sb.Append(expr.Value);
                        break;
                }
            }
            
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// 执行编译。
        /// </summary>
        /// <param name="querySpecification">查询规约。</param>
        /// <returns></returns>
        protected override string PerformCompile(QuerySpecification querySpecification)
        {
            var objectStack = new Stack<object>();
            var queryStack = new Stack<string>();

            var visitor = new DelegatedQuerySpecificationVisitor(
                querySpecification,
                objectStack.Push,
                objectStack.Push,
                objectStack.Push);
            visitor.Visit();

            while (objectStack.Count > 0)
            {
                var item = objectStack.Pop();
                if (item is Expression)
                {
                    queryStack.Push(this.GetExpressionPresentation(item as Expression));
                }
                else if (item is UnaryLogicalOperation)
                {
                    var unaryLogicalOperation = item as UnaryLogicalOperation;
                    var cachedQuery = queryStack.Pop();
                    queryStack.Push(
                        string.Format("({0} {1})", unaryLogicalOperation.Operator.ToString().ToUpper(), cachedQuery));
                }
                else if (item is LogicalOperation)
                {
                    var logicalOperation = item as LogicalOperation;
                    var leftQuery = queryStack.Pop();
                    var rightQuery = queryStack.Pop();
                    queryStack.Push(
                        string.Format(
                            "({0} {1} {2})",
                            leftQuery,
                            logicalOperation.Operator.ToString().ToUpper(),
                            rightQuery));
                }
            }

            return queryStack.Pop();
        }
    }
}
