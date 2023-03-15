using MassTransit;
using MicroserviceSagaChoreographyPattern.Order.API.Models;
using MicroserviceSagaChoreographyPattern.Shared.Events.Payment;
using MicroserviceSagaChoreographyPattern.Shared.Events.Stock;

namespace MicroserviceSagaChoreographyPattern.Order.API.Consumers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentCompletedEvent> _logger;
        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order != null)
            {
                order.OrderStatus = OrderStatus.Failed;
                order.FailMessage = context.Message.Description;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Order (Id={context.Message.OrderId}) status changed : {order.OrderStatus}");

            }
            else
            {
                _logger.LogInformation($"Order (Id={context.Message.OrderId}) not found");
            }
        }
    }
}
