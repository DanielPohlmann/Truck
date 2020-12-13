using System;
using Volvo.Core.DomainObject;

namespace Volvo.Trucks.API.Models
{
    public class Truck : Entity, IAggregateRoot
    {  
        protected Truck() { }

        public Truck(Guid modelId, int manufactureYear, string vIN)
        {
            ModelId = modelId;
            ManufactureYear = manufactureYear;
            VIN = new Vin(vIN);
        }

        public Guid ModelId { get; private set; }
        public virtual Model Model { get; private set; }
        public int ManufactureYear { get; private set; }
        public Vin VIN { get; private set; }

        public void AddModel(Model model) {
            Model = model;
        }

    }
}
