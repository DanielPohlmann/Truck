using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Volvo.Trucks.API.Application.Events
{
    public class TruckEventHandler : INotificationHandler<TruckRegistrationEvent>
    {
        public Task Handle(TruckRegistrationEvent notification, CancellationToken cancellationToken)
        {
            // Enviar evento (email, push, message, whattsapp)
            return Task.CompletedTask;
        }
    }
}
