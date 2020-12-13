using System.Collections.Generic;
using System.Threading.Tasks;
using Volvo.Trucks.API.Application.Queries.Interfaces;
using Volvo.Trucks.API.Models;
using Volvo.Trucks.API.Models.Interfaces;

namespace Volvo.Trucks.API.Application.Queries
{
    public class ModelServiceQuery : IModelServiceQuery
    {
        private readonly IModelRepository _modelRepository;

        public ModelServiceQuery(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<IEnumerable<Model>> GetAll() => await _modelRepository.GetAll();
    }
}
