using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volvo.Core.Data;
using Volvo.Core.DomainObject;
using Volvo.Core.Utils;
using Volvo.Trucks.API.Utils;

namespace Volvo.Trucks.API.Models.Interfaces
{
    public interface ITruckRepository : IRepository<Truck>
    {
        Task<IEnumerable<Truck>> GetAll(TruckFilter filter);
        Task<Truck> GetById(Guid id);
        Task<Truck> GetByVIN(string number);
        void Remove(Truck truck);
        void Add(Truck truck);
        void Edit(Truck truck);

    }
}
