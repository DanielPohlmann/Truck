using FluentValidation.Results;
using System.Threading.Tasks;
using Volvo.Core.Messages;

namespace Volvo.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;
        Task<ValidationResult> SendCommand<T>(T comando) where T : Command;
    }
}
