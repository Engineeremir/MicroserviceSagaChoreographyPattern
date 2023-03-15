using MassTransit;
using MicroserviceSagaChoreographyPattern.Shared.Events.Payment;
using MicroserviceSagaChoreographyPattern.Stock.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceSagaChoreographyPattern.Stock.API.Consumers
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
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                if (stock != null)
                {
                    stock.Count += item.Count; ;
                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation($"Stock was released for Order Id: {context.Message.OrderId}");
            }
        }
    }
}
