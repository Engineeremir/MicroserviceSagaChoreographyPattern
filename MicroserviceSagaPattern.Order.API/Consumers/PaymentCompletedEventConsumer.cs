using MassTransit;
using MicroserviceSagaPattern.Shared.Events.Payment;

namespace MicroserviceSagaPattern.Order.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;
        public Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
