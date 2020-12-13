using FluentValidation;
using System;
using Volvo.Core.Messages;
using Volvo.Core.Resources;

namespace Volvo.Trucks.API.Application.Commands
{
    public class EditTruckCommand : Command
    {
        public EditTruckCommand()
        {
        }

        public EditTruckCommand(Guid id, Guid modelId, int manufactureYear, string vin)
        {
            Id = id;
            ModelId = modelId;
            ManufactureYear = manufactureYear;
            Vin = vin;
        }

        public Guid Id { get; set; }
        public Guid ModelId { get; set; }
        public int ManufactureYear { get; set; }
        public string Vin { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new EditTruckValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class EditTruckValidation : AbstractValidator<EditTruckCommand>
        {
            private const int ManufactureYearMinLength = 1927;

            public EditTruckValidation()
            {
                RuleFor(c => c.Id)
                    .NotNull()
                    .NotEqual(Guid.Empty)
                    .WithMessage(MessagesValidation.IdRequerid);

                RuleFor(c => c.ModelId)
                    .NotNull()
                    .NotEqual(Guid.Empty)
                    .WithMessage(MessagesValidation.ModelIdRequerid);

                RuleFor(c => c.ManufactureYear)
                    .GreaterThanOrEqualTo(ManufactureYearMinLength)
                    .WithMessage(string.Format(MessagesValidation.ManufactureYearMinLength, ManufactureYearMinLength));

                RuleFor(c => c.Vin)
                  .Must(ValidateVin)
                  .WithMessage(MessagesValidation.VinInvalid);
            }

            protected static bool ValidateVin(string vin)
            {
                return Core.DomainObject.Vin.Validate(vin);
            }
        }
    }
}
