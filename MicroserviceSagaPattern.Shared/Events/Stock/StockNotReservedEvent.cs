using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceSagaChoreographyPattern.Shared.Events.Stock
{
    public class StockNotReservedEvent
    {
        public int OrderId { get; set; }
        public string Description { get; set; }

    }
}
