using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceSagaPattern.Shared.Events.Order
{
    public class StockNotReservedEvent
    {
        public int OrderId { get; set; }
        public string Description { get; set; }

    }
}
