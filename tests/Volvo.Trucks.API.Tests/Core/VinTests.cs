using Bogus;
using System.Threading.Tasks;
using Volvo.Core.DomainObject;
using Xunit;

namespace Volvo.Trucks.API.Tests.Core
{
    public class VinTests
    {
        private readonly Faker _faker;
        public VinTests()
        {
            _faker = new Faker();
        }

        [Theory(DisplayName = "Null Or Empaty Vin")]
        [Trait("Category", "Vin")]
        [InlineData(null)]
        [InlineData("")]
        public async Task NullOrEmpatyVin(string number)
        {
            Assert.ThrowsAny<DomainException>(() => new Vin(number));
        }

        [Fact(DisplayName = "Invalid Vin")]
        [Trait("Category", "Vin")]
        public async Task InvalidVin()
        {
            Assert.ThrowsAny<DomainException>(() => new Vin(_faker.Random.String(1, 17)));
        }
    }
}
