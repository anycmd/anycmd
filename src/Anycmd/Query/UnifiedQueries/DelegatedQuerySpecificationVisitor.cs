
namespace Anycmd.Query.UnifiedQueries
{
    using System;

    public class DelegatedQuerySpecificationVisitor : QuerySpecificationVisitor
    {
        private readonly Action<Expression> _visitExpression;

        private readonly Action<LogicalOperation> _visitLogicalOperation;

        private readonly Action<UnaryLogicalOperation> _visitUnaryLogicalOperation;

        public DelegatedQuerySpecificationVisitor(
            QuerySpecification querySpecification,
            Action<Expression> visitExpression,
            Action<LogicalOperation> visitLogicalOperation,
            Action<UnaryLogicalOperation> visitUnaryLogicalOperation)
            : base(querySpecification)
        {
            this._visitExpression = visitExpression;
            this._visitLogicalOperation = visitLogicalOperation;
            this._visitUnaryLogicalOperation = visitUnaryLogicalOperation;
        }

        protected override void VisitExpression(Expression expression)
        {
            if (this._visitExpression != null)
            {
                this._visitExpression(expression);
            }
        }

        protected override void VisitLogicalOperation(LogicalOperation logicalOperation)
        {
            if (this._visitLogicalOperation != null)
            {
                this._visitLogicalOperation(logicalOperation);
            }
        }

        protected override void VisitUnaryLogicalOperation(UnaryLogicalOperation unaryLogicalOperation)
        {
            if (this._visitUnaryLogicalOperation != null)
            {
                this._visitUnaryLogicalOperation(unaryLogicalOperation);
            }
        }
    }
}
