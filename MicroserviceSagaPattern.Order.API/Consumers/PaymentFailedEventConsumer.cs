using MassTransit;
using MicroserviceSagaPattern.Order.API.Models;
using MicroserviceSagaPattern.Shared.Events.Payment;

namespace MicroserviceSagaPattern.Order.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentCompletedEvent> _logger;

        public PaymentFailedEventConsumer(ApplicationDbContext context, ILogger<PaymentCompletedEvent> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
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
