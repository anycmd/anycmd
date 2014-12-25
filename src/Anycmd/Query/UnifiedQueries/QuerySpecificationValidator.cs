namespace Anycmd.Query.UnifiedQueries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnifiedQueries.Validators;

    public abstract class QuerySpecificationValidator : QuerySpecificationVisitor
    {
        private readonly List<string> _errorMessages = new List<string>();

        protected QuerySpecificationValidator(QuerySpecification querySpecification)
            : base(querySpecification)
        {
        }

        public bool HasError { get; private set; }

        public IEnumerable<string> ErrorMessages
        {
            get { return this._errorMessages; }
        }

        public int ErrorCount
        {
            get { return this._errorMessages.Count; }
        }

        public static bool Validate(QuerySpecification querySpecification, bool throwIfHasError = false)
        {
            var validators = new QuerySpecificationValidator[]
            {
                new ExpressionNameTypeValidator(querySpecification)
            };

            return Validate(throwIfHasError, validators);
        }

        public bool Validate(bool throwIfHasError = false)
        {
            this.Visit();
            if (this.HasError && throwIfHasError)
            {
                throw new InvalidOperationException(this.ToString());
            }

            return !this.HasError;
        }

        public override string ToString()
        {
            if (!this.HasError)
            {
                return "Succeeded";
            }

            var sb = new StringBuilder();
            sb.AppendLine("Validation Failed.");
            this._errorMessages.ForEach(p => sb.AppendLine(p));
            return sb.ToString();
        }

        protected void AddError(string message)
        {
            this._errorMessages.Add(message);
            this.HasError = true;
        }

        private static bool Validate(bool throwIfHasError = false, params QuerySpecificationValidator[] validators)
        {
            if (validators == null || validators.Length == 0)
            {
                throw new ArgumentNullException("validators");
            }

            return validators.Aggregate(true, (current, validator) => current && validator.Validate(throwIfHasError));
        }
    }
}
