using MassTransit;
using MicroserviceSagaPattern.Order.API.Models;
using MicroserviceSagaPattern.Shared.Events.Payment;

namespace MicroserviceSagaPattern.Order.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<PaymentCompletedEvent> _logger;

        public PaymentCompletedEventConsumer(ApplicationDbContext context, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint, ILogger<PaymentCompletedEvent> logger)
        {
            _context = context;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order != null)
            {
                order.OrderStatus = OrderStatus.Completed;
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
