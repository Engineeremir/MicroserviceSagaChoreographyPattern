using MicroserviceSagaChoreographyPattern.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceSagaChoreographyPattern.Shared.Events.Payment
{
    public class PaymentFailedEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public string Description { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
