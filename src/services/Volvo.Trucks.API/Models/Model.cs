using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volvo.Core.DomainObject;

namespace Volvo.Trucks.API.Models
{
    public class Model : Entity, IAggregateRoot
    {
        protected Model() { }

        public Model(string name, int modelYear)
        {
            Name = name;
            ModelYear = modelYear;
        }

        public string Name { get; private set; }
        public int ModelYear { get; private set; }
        public virtual ICollection<Truck> Trucks { get; private set; }
    }
}
