using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volvo.Core.Data;
using Volvo.Trucks.API.Models;
using Volvo.Trucks.API.Models.Interfaces;
using Volvo.Trucks.API.Utils;

namespace Volvo.Trucks.API.Data.Repository
{
    public class TruckRepository : ITruckRepository
    {
        private readonly TruckContext _context;

        public TruckRepository(TruckContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;


        public async Task<IEnumerable<Truck>> GetAll(TruckFilter filter)
        {
            var query = _context.Trucks.Join(_context.Models,
                     s => s.ModelId,
                     sa => sa.Id,
                     (s, sa) => new { truck = s, model = sa })
                .AsNoTracking();

            if (filter.ModelId != null && filter.ModelId != Guid.Empty)
                query = query.Where(x => x.truck.ModelId == filter.ModelId);

            query = query.OrderBy(x => x.truck.VIN.Number).Skip(filter.Skip).Take(filter.Take);

            var result = await query.ToListAsync();
            var resultTrucks = result.Select(x => x.truck).Distinct();
            var resultModels = result.Select(x => x.model).Distinct();
            resultTrucks = resultTrucks.Join(resultModels, t => t.ModelId, m => m.Id, (t, m) =>
            {
                t.AddModel(m);
                return t;
            });

            return resultTrucks;
        }

        public Task<Truck> GetById(Guid id)
        {
            return _context.Trucks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Truck> GetByVIN(string number)
        {
            return _context.Trucks.FirstOrDefaultAsync(x => x.VIN.Number == number);
        }

        public void Remove(Truck truck)
        {
            _context.Trucks.Remove(truck);
        }

        public void Add(Truck truck)
        {
            _context.Trucks.Add(truck);
        }

        public void Edit(Truck truck)
        {
            _context.Trucks.Update(truck);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
