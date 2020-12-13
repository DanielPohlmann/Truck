using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using Volvo.Core.Messages;
using Volvo.Core.Resources;

namespace Volvo.Trucks.API.Application.Commands
{
    public class RemoveTruckCommand : Command
    {
        public RemoveTruckCommand() { }

        public RemoveTruckCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new RemoveTruckValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class RemoveTruckValidation : AbstractValidator<RemoveTruckCommand>
        {
            public RemoveTruckValidation()
            {
                RuleFor(c => c.Id)
                    .NotNull()
                    .NotEqual(Guid.Empty)
                    .WithMessage(MessagesValidation.IdRequerid);
            }

        }
    }
}
