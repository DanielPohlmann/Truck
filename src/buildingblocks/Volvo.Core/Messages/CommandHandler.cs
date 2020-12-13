using FluentValidation.Results;
using System.Threading.Tasks;
using Volvo.Core.Data;
using Volvo.Core.Resources;

namespace Volvo.Core.Messages
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string message)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
        }

        protected async Task<ValidationResult> PersistData(IUnitOfWork uow)
        {
            if (!await uow.Commit()) AddError(MessagesValidation.ErrorDataPersist);

            return ValidationResult;
        }
    }
}
