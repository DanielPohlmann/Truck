using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volvo.Core.Data;
using Volvo.Trucks.API.Models;
using Volvo.Trucks.API.Models.Interfaces;

namespace Volvo.Trucks.API.Data.Repository
{
    public class ModelRepository : IModelRepository
    {
        private readonly TruckContext _context;

        public ModelRepository(TruckContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Model>> GetAll()
        {
            return await _context.Models.AsNoTracking().ToListAsync();
        }

        public Task<Model> GetById(Guid id)
        {
            return _context.Models.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
