using Bogus;
using MediatR;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volvo.Core.Mediator;
using Volvo.Core.Messages;
using Volvo.Core.Resources;
using Volvo.Trucks.API.Application.Commands;
using Volvo.Trucks.API.Application.Events;
using Volvo.Trucks.API.Models;
using Volvo.Trucks.API.Models.Interfaces;
using Xunit;

namespace Volvo.Trucks.API.Tests.Application
{
    public class TruckCommandHandlerTests
    {
        private readonly Mock<IModelRepository> _modelRepository;
        private readonly Mock<ITruckRepository> _truckRepository;

        private readonly Faker _faker;
        private const int MinYear = 1927;

        private readonly TruckCommandHandler _handler;
        public TruckCommandHandlerTests()
        {
            _modelRepository = new Mock<IModelRepository>();
            _truckRepository = new Mock<ITruckRepository>();
            _faker = new Faker();
            MessagesValidation.Culture = new System.Globalization.CultureInfo("pt");
            _handler = new TruckCommandHandler(_truckRepository.Object, _modelRepository.Object);
        }

        #region AddTruckCommand

        [Fact(DisplayName = "Add Truck Paramenter Invalid")]
        [Trait("Category", "AddTruckCommand")]
        public async Task AddTruckParamenterInvalid()
        {
            //Arange           
            AddTruckCommand command = new AddTruckCommand();

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.Contains(MessagesValidation.ModelIdRequerid, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(string.Format(MessagesValidation.ManufactureYearMinLength, MinYear), result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(MessagesValidation.VinInvalid, result.Errors.Select(x => x.ErrorMessage));

        }

        [Fact(DisplayName = "Add Truck Model Not Exist")]
        [Trait("Category", "AddTruckCommand")]
        public async Task AddTruckModelNotExist()
        {
            //Arange           
            AddTruckCommand command = GetAddTruckCommand;
            _modelRepository.Setup(x => x.GetById(command.ModelId)).Returns(Task.FromResult<Model>(null));

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.Contains(MessagesValidation.ModelNotExist, result.Errors.Select(x => x.ErrorMessage));

        }

        [Fact(DisplayName = "Add Truck Model Yer Manufacture Year Invalid")]
        [Trait("Category", "AddTruckCommand")]
        public async Task AddTruckModelYerManufactureYearInvalid()
        {
            //Arange         
            
            AddTruckCommand command = GetAddTruckCommand;
            var model = GetModel;
            command.ManufactureYear = model.ModelYear + _faker.Random.Int(2);
            _modelRepository.Setup(x => x.GetById(command.ModelId)).Returns(Task.FromResult(GetModel));

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.Contains(MessagesValidation.ModelYerManufactureYearValidate, result.Errors.Select(x => x.ErrorMessage));
        }



        [Fact(DisplayName = "Add Truck Exist")]
        [Trait("Category", "AddTruckCommand")]
        public async Task AddTruckExist()
        {
            //Arange      
            AddTruckCommand command = GetAddTruckCommand;
            var model = GetModel;
            command.ManufactureYear = model.ModelYear;
            _modelRepository.Setup(x => x.GetById(command.ModelId)).Returns(Task.FromResult(model));
            _truckRepository.Setup(x => x.GetByVIN(command.Vin)).Returns(Task.FromResult(GetTruck));

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.Contains(MessagesValidation.TruckExist, result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact(DisplayName = "Add Truck Error Pessist Data")]
        [Trait("Category", "AddTruckCommand")]
        public async Task AddTruckErrorPessistData()
        {
            //Arange  
            AddTruckCommand command = GetAddTruckCommand;
            var model = GetModel;
            command.ManufactureYear = model.ModelYear;
            _modelRepository.Setup(x => x.GetById(command.ModelId)).Returns(Task.FromResult(model));
            _truckRepository.Setup(x => x.GetByVIN(command.Vin)).Returns(Task.FromResult<Truck>(null));
            _truckRepository.Setup(x => x.Add(new Truck(command.ModelId, command.ManufactureYear, command.Vin)));
            _truckRepository.Setup(c => c.UnitOfWork.Commit()).Returns(Task.FromResult<bool>(false));

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.Contains(MessagesValidation.ErrorDataPersist, result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact(DisplayName = "Add Truck Success")]
        [Trait("Category", "AddTruckCommand")]
        public async Task AddTruckSuccess()
        {
            //Arange 
            AddTruckCommand command = GetAddTruckCommand;
            var model = GetModel;
            command.ManufactureYear = model.ModelYear;
            _modelRepository.Setup(x => x.GetById(command.ModelId)).Returns(Task.FromResult(model));
            _truckRepository.Setup(x => x.GetByVIN(command.Vin)).Returns(Task.FromResult<Truck>(null));
            _truckRepository.Setup(x => x.Add(new Truck(command.ModelId, command.ManufactureYear, command.Vin)));
            _truckRepository.Setup(c => c.UnitOfWork.Commit()).Returns(Task.FromResult<bool>(true));

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.False(result.Errors.Any());
        }

        #endregion


        #region EditTruckCommand

        [Fact(DisplayName = "Edit Truck Paramenter Invalid")]
        [Trait("Category", "EditTruckCommand")]
        public async Task EditTruckParamenterInvalid()
        {
            //Arange           
            EditTruckCommand command = new EditTruckCommand();

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.Contains(MessagesValidation.IdRequerid, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(MessagesValidation.ModelIdRequerid, result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(string.Format(MessagesValidation.ManufactureYearMinLength, MinYear), result.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(MessagesValidation.VinInvalid, result.Errors.Select(x => x.ErrorMessage));

        }

        [Fact(DisplayName = "Edit Truck Model Not Exist")]
        [Trait("Category", "EditTruckCommand")]
        public async Task EditTruckModelNotExist()
        {
            //Arange           
            EditTruckCommand command = GetEditTruckCommand;
            _modelRepository.Setup(x => x.GetById(command.ModelId)).Returns(Task.FromResult<Model>(null));

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.Contains(MessagesValidation.ModelNotExist, result.Errors.Select(x => x.ErrorMessage));

        }

        [Fact(DisplayName = "Edit Truck Model Yer Manufacture Year Invalid")]
        [Trait("Category", "EditTruckCommand")]
        public async Task EditTruckModelYerManufactureYearInvalid()
        {
            //Arange         

            EditTruckCommand command = GetEditTruckCommand;
            var model = GetModel;
            command.ManufactureYear = model.ModelYear + _faker.Random.Int(2);
            _modelRepository.Setup(x => x.GetById(command.ModelId)).Returns(Task.FromResult(GetModel));

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.Contains(MessagesValidation.ModelYerManufactureYearValidate, result.Errors.Select(x => x.ErrorMessage));
        }



        [Fact(DisplayName = "Edit Truck Not Exist")]
        [Trait("Category", "EditTruckCommand")]
        public async Task EditTruckNotExist()
        {
            //Arange      
            EditTruckCommand command = GetEditTruckCommand;
            var model = GetModel;
            command.ManufactureYear = model.ModelYear;
            _modelRepository.Setup(x => x.GetById(command.ModelId)).Returns(Task.FromResult(model));
            _truckRepository.Setup(x => x.GetById(command.Id)).Returns(Task.FromResult<Truck>(null));

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.Contains(MessagesValidation.TruckNotExist, result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact(DisplayName = "Edit Truck Error Pessist Data")]
        [Trait("Category", "EditTruckCommand")]
        public async Task EditTruckErrorPessistData()
        {
            //Arange  
            EditTruckCommand command = GetEditTruckCommand;
            var model = GetModel;
            command.ManufactureYear = model.ModelYear;
            _modelRepository.Setup(x => x.GetById(command.ModelId)).Returns(Task.FromResult(model));
            _truckRepository.Setup(x => x.GetById(command.Id)).Returns(Task.FromResult<Truck>(GetTruck));
            _truckRepository.Setup(x => x.Edit(new Truck(command.ModelId, command.ManufactureYear, command.Vin) { Id = command.Id }));
            _truckRepository.Setup(c => c.UnitOfWork.Commit()).Returns(Task.FromResult<bool>(false));

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.Contains(MessagesValidation.ErrorDataPersist, result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact(DisplayName = "Edit Truck Success")]
        [Trait("Category", "EditTruckCommand")]
        public async Task EditTruckSuccess()
        {
            //Arange 
            EditTruckCommand command = GetEditTruckCommand;
            var model = GetModel;
            command.ManufactureYear = model.ModelYear;
            _modelRepository.Setup(x => x.GetById(command.ModelId)).Returns(Task.FromResult(model));
            _truckRepository.Setup(x => x.GetById(command.Id)).Returns(Task.FromResult<Truck>(GetTruck));
            _truckRepository.Setup(x => x.Edit(new Truck(command.ModelId, command.ManufactureYear, command.Vin) { Id = command.Id }));
            _truckRepository.Setup(c => c.UnitOfWork.Commit()).Returns(Task.FromResult<bool>(true));

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.False(result.Errors.Any());
        }

        #endregion



        #region RemoveTruckCommand

        [Fact(DisplayName = "Remove Truck Paramenter Id IsNull")]
        [Trait("Category", "RemoveTruckCommand")]
        public async Task RemoveTruckParamenterIdIsNull()
        {
            //Arange
            //var mediator = new Mock<IMediator>();
            RemoveTruckCommand command = new RemoveTruckCommand();

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.Contains(MessagesValidation.IdRequerid, result.Errors.Select(x => x.ErrorMessage));

            //something like:
            //mediator.Verify(x => x.Publish(It.IsAny<RemoveTruckCommand>()));
        }

        [Fact(DisplayName = "Remove Truck Not Find Truck")]
        [Trait("Category", "RemoveTruckCommand")]
        public async Task RemoveTruckNotFindTruck()
        {
            //Arange
            RemoveTruckCommand command = new RemoveTruckCommand() { Id = _faker.Random.Guid() };
            _truckRepository.Setup(c => c.GetById(command.Id)).Returns(Task.FromResult<Truck>(null));

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.Contains(MessagesValidation.TruckNotExist, result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact(DisplayName = "Remove Truck Error Persist Data")]
        [Trait("Category", "RemoveTruckCommand")]
        public async Task RemoveErrorPersistData()
        {
            //Arange
            RemoveTruckCommand command = new RemoveTruckCommand(_faker.Random.Guid());
            var truck = GetTruck;
            truck.Id = command.Id;
            _truckRepository.Setup(c => c.GetById(command.Id)).Returns(Task.FromResult<Truck>(truck));
            _truckRepository.Setup(c => c.Remove(truck));
            _truckRepository.Setup(c => c.UnitOfWork.Commit()).Returns(Task.FromResult<bool>(false));

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.Contains(MessagesValidation.ErrorDataPersist, result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact(DisplayName = "Remove Truck Success")]
        [Trait("Category", "RemoveTruckCommand")]
        public async Task RemoveTruckSuccess()
        {
            //Arange
            RemoveTruckCommand command = new RemoveTruckCommand(_faker.Random.Guid());
            var truck = GetTruck;
            truck.Id = command.Id;
            _truckRepository.Setup(c => c.GetById(command.Id)).Returns(Task.FromResult<Truck>(truck));
            _truckRepository.Setup(c => c.Remove(truck));
            _truckRepository.Setup(c => c.UnitOfWork.Commit()).Returns(Task.FromResult<bool>(true));

            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());

            //Asert
            Assert.False(result.Errors.Any());
        }
        #endregion


        private Truck GetTruck => new Truck(new Guid(), MinYear, _faker.Vehicle.Vin()) { Id = _faker.Random.Guid() };
        private Model GetModel => new Model(_faker.Random.String(10), _faker.Random.Int(MinYear, DateTime.Now.Year)) { Id = _faker.Random.Guid() };
        private AddTruckCommand GetAddTruckCommand => new AddTruckCommand(_faker.Random.Guid(), _faker.Random.Int(MinYear, DateTime.Now.Year), _faker.Vehicle.Vin());
        private EditTruckCommand GetEditTruckCommand => new EditTruckCommand(_faker.Random.Guid(), _faker.Random.Guid(), _faker.Random.Int(MinYear, DateTime.Now.Year), _faker.Vehicle.Vin());

    }
}
