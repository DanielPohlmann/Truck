using FluentValidation;
using System;
using Volvo.Core.Messages;
using Volvo.Core.Resources;

namespace Volvo.Trucks.API.Application.Commands
{
    public class AddTruckCommand : Command
    {
        public AddTruckCommand()
        {
        }

        public AddTruckCommand(Guid modelId, int manufactureYear, string vin) : this()
        {           
            ModelId = modelId;
            ManufactureYear = manufactureYear;
            Vin = vin;
        }

        public Guid ModelId { get; set; }
        public int ManufactureYear { get; set; }
        public string Vin { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AddTruckValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AddTruckValidation : AbstractValidator<AddTruckCommand>
        {
            private const int ManufactureYearMinLength = 1927;

            public AddTruckValidation()
            {
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
