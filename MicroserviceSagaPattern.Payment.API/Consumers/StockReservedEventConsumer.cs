using MassTransit;
using MicroserviceSagaChoreographyPattern.Shared.Events.Payment;
using MicroserviceSagaChoreographyPattern.Shared.Events.Stock;

namespace MicroserviceSagaChoreographyPattern.Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {

        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<StockReservedEvent> _logger;

        public StockReservedEventConsumer(ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint, ILogger<StockReservedEvent> logger)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var balance = 3000m;
            if (balance > context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdrawn from your card.  UserID: {context.Message.BuyerId}");
                await _publishEndpoint.Publish(new PaymentCompletedEvent
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                });
            }

            else
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL can not be able to withdrawn from your card.  UserID: {context.Message.BuyerId}");
                await _publishEndpoint.Publish(new PaymentFailedEvent
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Description = "Payment Failed",
                    OrderItems = context.Message.OrderItems,
                });
            }
        }
    }
}
