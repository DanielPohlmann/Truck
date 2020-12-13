using System.Collections.Generic;
using System.Threading.Tasks;
using Volvo.Trucks.API.Models;

namespace Volvo.Trucks.API.Application.Queries.Interfaces
{
    public interface IModelServiceQuery
    {
        Task<IEnumerable<Model>> GetAll();
    }
}
