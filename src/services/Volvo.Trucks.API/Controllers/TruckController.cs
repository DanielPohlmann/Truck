using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volvo.Core.Mediator;
using Volvo.Trucks.API.Application.Commands;
using Volvo.Trucks.API.Application.Queries.Interfaces;
using Volvo.Trucks.API.Utils;
using Volvo.WebApi.Core.Controllers;

namespace Volvo.Trucks.API.Controllers
{
    public class TruckController : MainController
    {

        private readonly IMediatorHandler _mediator;
        private readonly ITruckServiceQuery _truckServiceQuery;

        public TruckController(IMediatorHandler mediator, ITruckServiceQuery truckServiceQuery)
        {
            _mediator = mediator;
            _truckServiceQuery = truckServiceQuery;
        }

        [HttpGet("truck/getAll")]
        public async Task<IActionResult> GetAllTruck(int page, Guid? ModelId = null) {
            var result = await _truckServiceQuery.GetAll(new TruckFilter() { Page = page, ModelId = ModelId.GetValueOrDefault() });
            return result == null ? NotFound() : CustomResponse(result);
        }       

        [HttpPost("truck/create")]
        public async Task<IActionResult> CreateTruck(AddTruckCommand truck) => CustomResponse(await _mediator.SendCommand(truck));

        [HttpPut("truck/edit")]
        public async Task<IActionResult> EditTruck(EditTruckCommand truck) => CustomResponse(await _mediator.SendCommand(truck));
      
        [HttpDelete("truck/delete")]
        public async Task<IActionResult> RemoveTruck(RemoveTruckCommand truck) => CustomResponse(await _mediator.SendCommand(truck));
    }
}
