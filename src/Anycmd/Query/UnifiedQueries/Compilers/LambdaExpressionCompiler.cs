namespace Anycmd.Query.UnifiedQueries.Compilers
{
    using System;
    using System.Collections.Generic;
    using LinqExpressions = System.Linq.Expressions;

    public class LambdaExpressionCompiler<T> : QuerySpecificationCompiler<LinqExpressions.Expression<Func<T, bool>>>
    {
        /// <summary>
        /// 执行编译。
        /// </summary>
        /// <param name="querySpecification">查询规约。</param>
        /// <returns>LINQ断言表达式</returns>
        protected override LinqExpressions.Expression<Func<T, bool>> PerformCompile(QuerySpecification querySpecification)
        {
            var type = typeof(T); // new DynamicTypeBuilder(querySpecification).BuildType();

            var objectStack = new Stack<object>();
            var visitor = new DelegatedQuerySpecificationVisitor(
                querySpecification,
                objectStack.Push,
                objectStack.Push,
                objectStack.Push);
            visitor.Visit();

            var parameterExpression = LinqExpressions.Expression.Parameter(type, "p");
            var expressionStack = new Stack<LinqExpressions.Expression>();

            while (objectStack.Count > 0)
            {
                var item = objectStack.Pop();
                if (item is Expression)
                {
                    var e = item as Expression;
                    var propertyExpression = LinqExpressions.Expression.Property(parameterExpression, e.Name);
                    var constantExpression = LinqExpressions.Expression.Constant(e.GetValue());
                    if (e.Type == DataTypes.String)
                    {
                        var methodInfo = typeof(string).GetMethod(e.Operator.ToString(), new[] { typeof(string) });
                        expressionStack.Push(LinqExpressions.Expression.Call(propertyExpression, methodInfo, constantExpression));
                    }
                    else
                    {
                        Func<LinqExpressions.Expression, LinqExpressions.Expression, LinqExpressions.BinaryExpression> relationalOperationExpression;
                        switch (e.Operator)
                        {
                            case RelationalOperators.EqualTo:
                                relationalOperationExpression = LinqExpressions.Expression.Equal;
                                break;
                            case RelationalOperators.GreaterThan:
                                relationalOperationExpression = LinqExpressions.Expression.GreaterThan;
                                break;
                            case RelationalOperators.GreaterThanOrEqualTo:
                                relationalOperationExpression = LinqExpressions.Expression.GreaterThanOrEqual;
                                break;
                            case RelationalOperators.LessThan:
                                relationalOperationExpression = LinqExpressions.Expression.LessThan;
                                break;
                            case RelationalOperators.LessThanOrEqualTo:
                                relationalOperationExpression = LinqExpressions.Expression.LessThanOrEqual;
                                break;
                            default:
                                throw new NotSupportedException(string.Format("The relational operator {0} is not supported.", e.Operator));
                        }

                        expressionStack.Push(GetRelationalExpression(propertyExpression, constantExpression, relationalOperationExpression));
                    }
                }
                else if (item is UnaryLogicalOperation)
                {
                    var unaryLogicalOperation = item as UnaryLogicalOperation;
                    var e = expressionStack.Pop();
                    switch (unaryLogicalOperation.Operator)
                    {
                        case UnaryLogicalOperators.Not:
                            expressionStack.Push(LinqExpressions.Expression.Not(e));
                            break;
                        default:
                            throw new NotSupportedException(string.Format("The relational operator {0} is not supported.", unaryLogicalOperation.Operator));
                    }
                }
                else if (item is LogicalOperation)
                {
                    var logicalOperation = item as LogicalOperation;
                    var e1 = expressionStack.Pop();
                    var e2 = expressionStack.Pop();
                    Func<LinqExpressions.Expression, LinqExpressions.Expression, LinqExpressions.BinaryExpression> relationalOperationExpression;
                    switch (logicalOperation.Operator)
                    {
                        case LogicalOperators.And:
                            relationalOperationExpression = LinqExpressions.Expression.AndAlso;
                            break;
                            case LogicalOperators.Or:
                            relationalOperationExpression = LinqExpressions.Expression.OrElse;
                            break;
                        default:
                            throw new NotSupportedException(string.Format("The relational operator {0} is not supported.", logicalOperation.Operator));
                    }

                    expressionStack.Push(relationalOperationExpression(e1, e2));
                }
            }

            var builtExpression = expressionStack.Pop();
            return LinqExpressions.Expression.Lambda<Func<T, bool>>(builtExpression, parameterExpression);
        }

        private static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static LinqExpressions.Expression GetRelationalExpression(
            LinqExpressions.Expression e1,
            LinqExpressions.Expression e2,
            Func<LinqExpressions.Expression, LinqExpressions.Expression, LinqExpressions.BinaryExpression> operation)
        {
            if (IsNullableType(e1.Type) && !IsNullableType(e2.Type))
            {
                e2 = LinqExpressions.Expression.Convert(e2, e1.Type);
            }
            else if (!IsNullableType(e1.Type) && IsNullableType(e2.Type))
            {
                e1 = LinqExpressions.Expression.Convert(e1, e2.Type);
            }

            return operation(e1, e2);
        }
    }
}
