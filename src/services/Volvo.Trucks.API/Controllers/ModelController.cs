using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volvo.Trucks.API.Application.Queries.Interfaces;
using Volvo.WebApi.Core.Controllers;

namespace Volvo.Trucks.API.Controllers
{
    public class ModelController : MainController
    {
        private readonly IModelServiceQuery _modelServiceQuery;

        public ModelController(IModelServiceQuery modelServiceQuery)
        {
            _modelServiceQuery = modelServiceQuery;
        }

        [HttpGet("model/getAll")]
        public async Task<IActionResult> GetAllModel() {
            var result = await _modelServiceQuery.GetAll();
            return result == null ? NotFound() : CustomResponse(result);
        } 

    }
}
