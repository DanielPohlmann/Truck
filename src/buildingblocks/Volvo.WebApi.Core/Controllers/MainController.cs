using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volvo.Core.Communication;

namespace Volvo.WebApi.Core.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {
        protected ICollection<string> Erros = new List<string>();

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperationIsValid())
            {
                return Ok(result ?? new { Success = true });
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", Erros.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                AddError(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected ActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
            {
                AddError(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected ActionResult CustomResponse<T>(ResponseResult<T> response) where T : class
        {
            if (response.ValidationResult.IsValid)
                return CustomResponse(response.Data);


            foreach (var erro in response.ValidationResult.Errors)
            {
                AddError(erro.ErrorMessage);
            }

            return CustomResponse();
        }      

        protected bool OperationIsValid()
        {
            return !Erros.Any();
        }

        protected void AddError(string erro)
        {
            Erros.Add(erro);
        }

        protected void ClearErrro()
        {
            Erros.Clear();
        }
    }
}