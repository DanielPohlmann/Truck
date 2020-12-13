using Bogus;
using ExpectedObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volvo.Core.Resources;
using Volvo.Trucks.API.Application.Queries;
using Volvo.Trucks.API.Models;
using Volvo.Trucks.API.Models.Interfaces;
using Volvo.Trucks.API.Utils;
using Xunit;

namespace Volvo.Trucks.API.Tests.Application
{
    public class ServiceQueryTests
    {
        private readonly ModelServiceQuery _modelServiceQuery;
        private readonly TruckServiceQuery _truckServiceQuery;

        private readonly Mock<IModelRepository> _modelRepository;
        private readonly Mock<ITruckRepository> _truckRepository;

        private readonly Faker _faker;

        private const int MinYear = 1927;

        public ServiceQueryTests() {
            _modelRepository = new Mock<IModelRepository>();
            _truckRepository = new Mock<ITruckRepository>();
            _truckServiceQuery = new TruckServiceQuery(_truckRepository.Object);
            _modelServiceQuery = new ModelServiceQuery(_modelRepository.Object);
            _faker = new Faker();
            MessagesValidation.Culture = new System.Globalization.CultureInfo("pt");
        }

        [Fact(DisplayName ="Get All Model Trucks")]
        [Trait("Category", "Query Model")]
        public async Task GetAllModel() {
            //Arrange
            var objectsExpected = Task.FromResult<IEnumerable<Model>>(
                new List<Model>() {  GetModel }
            );
            _modelRepository.Setup(x => x.GetAll()).Returns(objectsExpected);

            //Act
            var result = await _modelServiceQuery.GetAll();

            //Assert
            objectsExpected.GetAwaiter().GetResult().ToExpectedObject().ShouldEqual(result);
        }

        [Fact(DisplayName = "Get All Trucks Invalid Filter")]
        [Trait("Category", "Query Truck")]
        public async Task GetAllTruckInvalidFilter()
        {
            //Arrange
            var filter = new TruckFilter() { Page = _faker.Random.Int(-1000, -1) };

            //Act
            var result = await _truckServiceQuery.GetAll(filter);

            //Assert
            Assert.Contains(MessagesValidation.PaginateMinZero, result.ValidationResult.Errors.Select(x => x.ErrorMessage));
        }


        [Fact(DisplayName = "Get All Trucks")]
        [Trait("Category", "Query Truck")]
        public async Task GetAllTruck()
        {
            //Arrange
            var truck = new Truck(_faker.Random.Guid(), GetModel.ModelYear, _faker.Vehicle.Vin());
            truck.AddModel(GetModel);          
            var filter = new TruckFilter() { Page = _faker.Random.Int(0, int.MaxValue), ModelId = _faker.Random.Guid() };
            var objectsExpected = Task.FromResult<IEnumerable<Truck>>(
                new List<Truck>() { truck }
            );
            _truckRepository.Setup(x => x.GetAll(filter)).Returns(objectsExpected);

            //Act
            var result = await _truckServiceQuery.GetAll(filter);

            //Assert
            Assert.False(result.ValidationResult.Errors.Any());
            objectsExpected.GetAwaiter().GetResult().ToExpectedObject().ShouldEqual(result.Data);
            Assert.True(filter.Take == 25);
            Assert.True(filter.Skip == filter.Page * filter.Take);
        }


        private Model GetModel => new Model(_faker.Random.String(10), _faker.Random.Int(MinYear, DateTime.Now.Year)) { Id = _faker.Random.Guid() };




    }
}
