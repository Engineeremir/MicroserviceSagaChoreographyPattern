using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceSagaPattern.Shared
{
    public class RabbitMQSettings
    {
        public const string StockReservedQueueName = "stock-reserved-queue";
        public const string StockOrderOrderCreatedEventQueueName = "stock-order-created-queue";
    }
}
