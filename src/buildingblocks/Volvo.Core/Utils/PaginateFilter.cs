using FluentValidation;
using FluentValidation.Results;
using Volvo.Core.Resources;

namespace Volvo.Core.Utils
{
    public abstract class PaginateFilter 
    {
        public int Page { get; set; }

        public int Skip => Page * Take;


        public int Take => 25;

        public virtual ValidationResult IsValid() => new PaginateFilterValidate().Validate(this);       

        public class PaginateFilterValidate : AbstractValidator<PaginateFilter>
        {
            public PaginateFilterValidate()
            {
                RuleFor(c => c.Page)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage(MessagesValidation.PaginateMinZero);
            }
        }
    }
}
