using System;
using Volvo.Core.Utils;

namespace Volvo.Trucks.API.Utils
{
    public class TruckFilter : PaginateFilter
    {
        public Guid ModelId { get; set; }
    }
}
