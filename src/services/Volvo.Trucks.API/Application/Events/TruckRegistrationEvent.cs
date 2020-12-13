using System;
using Volvo.Core.Messages;

namespace Volvo.Trucks.API.Application.Events
{
    public class TruckRegistrationEvent : Event
    {
        public TruckRegistrationEvent(Guid modelId, int manufactureYear, string vIN)
        {
            ModelId = modelId;
            ManufactureYear = manufactureYear;
            VIN = vIN;
        }

        public Guid ModelId { get; private set; }
        public int ManufactureYear { get; private set; }
        public string VIN { get; private set; }
    }
}
