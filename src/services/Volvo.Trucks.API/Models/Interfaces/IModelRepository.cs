using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volvo.Core.Data;

namespace Volvo.Trucks.API.Models.Interfaces
{
    public interface IModelRepository : IRepository<Model>
    {
        Task<IEnumerable<Model>> GetAll();
        Task<Model> GetById(Guid id);
    }
}
