using MassTransit;
using MicroserviceSagaPattern.Shared.Events.Payment;
using MicroserviceSagaPattern.Shared.Events.Stock;

namespace MicroserviceSagaPattern.Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var balance = 3000m;
            if (balance>context.Message.Payment.TotalPrice)
            {
                await _publishEndpoint.Publish(new PaymentSuccessEvent
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                });
            }

            else
            {

            }
        }
    }
}
