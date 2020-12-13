using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Volvo.Core.Messages;
using Volvo.Core.Resources;
using Volvo.Trucks.API.Application.Events;
using Volvo.Trucks.API.Models;
using Volvo.Trucks.API.Models.Interfaces;

namespace Volvo.Trucks.API.Application.Commands
{
    public class TruckCommandHandler : CommandHandler,
        IRequestHandler<AddTruckCommand, ValidationResult>,
        IRequestHandler<EditTruckCommand, ValidationResult>,
        IRequestHandler<RemoveTruckCommand, ValidationResult>
    {
        private readonly ITruckRepository _truckRepository;
        private readonly IModelRepository _modelRepository;

        public TruckCommandHandler(ITruckRepository truckRepository, IModelRepository modelRepository)
        {
            _truckRepository = truckRepository;
            _modelRepository = modelRepository;
        }

        public async Task<ValidationResult> Handle(AddTruckCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            var truck = new Truck(request.ModelId, request.ManufactureYear, request.Vin);

            var validateModel = await ValidateModel(truck);
            if (!validateModel.IsValid)
                return validateModel;

            var truckExist = await _truckRepository.GetByVIN(truck.VIN.Number);
            if (truckExist != null)
            {
                AddError(MessagesValidation.TruckExist);
                return ValidationResult;
            }

            _truckRepository.Add(truck);

            truck.AddEvent(new TruckRegistrationEvent(request.ModelId, request.ManufactureYear, request.Vin));

            return await PersistData(_truckRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(EditTruckCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            var truck = new Truck(request.ModelId, request.ManufactureYear, request.Vin) { Id = request.Id };

            var validateModel = await ValidateModel(truck);
            if (!validateModel.IsValid)
                return validateModel;

            var truckExist = await _truckRepository.GetById(truck.Id);
            if (truckExist == null)
            {
                AddError(MessagesValidation.TruckNotExist);
                return ValidationResult;
            }

            _truckRepository.Edit(truck);

            return await PersistData(_truckRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(RemoveTruckCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            var truck = await _truckRepository.GetById(request.Id);
            if (truck == null)
            {
                AddError(MessagesValidation.TruckNotExist);
                return ValidationResult;
            }

            _truckRepository.Remove(truck);

            return await PersistData(_truckRepository.UnitOfWork);
        }


        private async Task<ValidationResult> ValidateModel(Truck truck) {

            var modelExist = await _modelRepository.GetById(truck.ModelId);
            if (modelExist == null)
            {
                AddError(MessagesValidation.ModelNotExist);
                return ValidationResult;
            }

            if (!(modelExist.ModelYear == truck.ManufactureYear || (modelExist.ModelYear + 1) == truck.ManufactureYear))
            {
                AddError(MessagesValidation.ModelYerManufactureYearValidate);
                return ValidationResult;
            }

            return ValidationResult;
        }
    }
}
