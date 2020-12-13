using FluentValidation.Results;
using System.Collections.Generic;

namespace Volvo.Core.Communication
{
    public class ResponseResult<T> where T : class
    {
        public ResponseResult()
        {
            ValidationResult = new ValidationResult();
        }

        public T Data { get; set; }
        public ValidationResult ValidationResult { get; set; }
    }
}
