using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volvo.Core.Communication;
using Volvo.Core.Utils;
using Volvo.Trucks.API.Models;
using Volvo.Trucks.API.Utils;

namespace Volvo.Trucks.API.Application.Queries.Interfaces
{
    public interface ITruckServiceQuery
    {
        Task<ResponseResult<IEnumerable<Truck>>> GetAll(TruckFilter filter);
    }
}
