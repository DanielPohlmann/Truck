using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volvo.Core.Communication;
using Volvo.Core.Resources;
using Volvo.Trucks.API.Application.Queries.Interfaces;
using Volvo.Trucks.API.Models;
using Volvo.Trucks.API.Models.Interfaces;
using Volvo.Trucks.API.Utils;

namespace Volvo.Trucks.API.Application.Queries
{
    public class TruckServiceQuery : ITruckServiceQuery
    {
        private readonly ITruckRepository _truckRepository;

        public TruckServiceQuery(ITruckRepository truckRepository)
        {
            _truckRepository = truckRepository;
        }

        public async Task<ResponseResult<IEnumerable<Truck>>> GetAll(TruckFilter filter) {

            var result = new ResponseResult<IEnumerable<Truck>>();

            var validate = filter.IsValid();
            if (!validate.IsValid) {
                result.ValidationResult = validate;
                return result;
            }

            result.Data = await _truckRepository.GetAll(filter);
            return result;

        }

    }
}
