namespace Anycmd.Query.UnifiedQueries.Validators
{
    using System.Collections.Generic;

    internal sealed class ExpressionNameTypeValidator : QuerySpecificationValidator
    {
        private readonly Dictionary<string, DataTypes> nameTypeDictionary = new Dictionary<string, DataTypes>();

        public ExpressionNameTypeValidator(QuerySpecification querySpecification)
            : base(querySpecification)
        {
        }

        protected override void VisitExpression(Expression expression)
        {
            if (this.nameTypeDictionary.ContainsKey(expression.Name))
            {
                var type = this.nameTypeDictionary[expression.Name];
                if (type != expression.Type)
                {
                    this.AddError(
                        string.Format(
                            "The data type defined on the expression '{0}' ('{1}') is not consistent with the previous definition ('{2}').",
                            expression.Name,
                            expression.Type,
                            type));
                }
            }
            else
            {
                this.nameTypeDictionary.Add(expression.Name, expression.Type);
            }
        }

        protected override void VisitLogicalOperation(LogicalOperation logicalOperation)
        {
        }

        protected override void VisitUnaryLogicalOperation(UnaryLogicalOperation unaryLogicalOperation)
        {
        }
    }
}
