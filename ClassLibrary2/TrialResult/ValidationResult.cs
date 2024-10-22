using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.TrialResult
{
    public sealed class ValidationResult<TData> : Result<TData>, IValidationResult
        where TData : class
    {
        private ValidationResult(Error[] errors)
            : base(data: null, false, IValidationResult.ValidationError) =>
                    Errors = errors;

        public Error[] Errors { get; }

        public static ValidationResult<TData> WithErrors(Error[] errors) => new(errors);
    }
}
