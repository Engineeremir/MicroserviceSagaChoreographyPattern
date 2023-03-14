using MassTransit;
using MicroserviceSagaPattern.Shared;
using MicroserviceSagaPattern.Shared.Events.Order;
using MicroserviceSagaPattern.Shared.Events.Stock;
using MicroserviceSagaPattern.Stock.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceSagaPattern.Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(ApplicationDbContext context, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var stockResult = new List<bool>();

            foreach (var  item in context.Message.OrderItems)
            {
                stockResult.Add(await _context.Stocks.AnyAsync(x=>x.ProductId==item.ProductId && x.Count>item.Count));
            }

            if (stockResult.All(x=>x.Equals(true)))
            {
                foreach (var item in context.Message.OrderItems)
                {
                    var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                    if (stock != null)
                    {
                        stock.Count -= item.Count;
                    }

                    await _context.SaveChangesAsync();
                }

                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StockReservedQueueName}"));

                StockReservedEvent stockReservedEvet = new StockReservedEvent()
                {
                    OrderId = context.Message.OrderId,
                    BuyerId= context.Message.BuyerId,
                    Payment = context.Message.Payment,
                    OrderItems = context.Message.OrderItems
                };

                await sendEndpoint.Send(stockReservedEvet);

            }
            else
            {
                await _publishEndpoint.Publish(new StockNotReservedEvent()
                {

                    OrderId = context.Message.OrderId,
                    Description = "Stock not found"
                });
            }
        }
    }
}
