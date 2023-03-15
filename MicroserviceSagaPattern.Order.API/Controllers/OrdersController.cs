using MassTransit;
using MicroserviceSagaChoreographyPattern.Order.API.Dtos;
using MicroserviceSagaChoreographyPattern.Order.API.Models;
using MicroserviceSagaChoreographyPattern.Shared.Events.Order;
using MicroserviceSagaChoreographyPattern.Shared.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceSagaChoreographyPattern.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPublishEndpoint _publishEndPoint;

        public OrdersController(ApplicationDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndPoint = publishEndpoint;
        }



        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto orderCreateDto)
        {
            var newOrder = new Models.Order
            {
                BuyerId = orderCreateDto.BuyerId,
                OrderStatus = OrderStatus.Suspended,
                Address = new Address
                {
                    Line = orderCreateDto.Address.Line,
                    District = orderCreateDto.Address.District,
                    Province = orderCreateDto.Address.Province,
                },
                CreatedDate = DateTime.Now,

            };

            orderCreateDto.OrderItems.ForEach(item =>
            {
                newOrder.OrderItems.Add(new OrderItem()
                {
                    Price = item.Price,
                    ProductId = item.ProductId,
                    Count = item.Count,
                });
            });

            await _context.AddAsync(newOrder);

            await _context.SaveChangesAsync();

            var orderCreatedEvent = new OrderCreatedEvent()
            {
                BuyerId = orderCreateDto.BuyerId,
                OrderId = newOrder.Id,
                Payment = new PaymentMessage()
                {
                    CardName = orderCreateDto.Payment.CardName,
                    CardNumber = orderCreateDto.Payment.CardNumber,
                    Expiration = orderCreateDto.Payment.Expiration,
                    CVV = orderCreateDto.Payment.CVV,
                    TotalPrice = orderCreateDto.OrderItems.Sum(x => x.Price * x.Count),
                }
            };

            orderCreateDto.OrderItems.ForEach(item =>
            {
                orderCreatedEvent.OrderItems.Add(new OrderItemMessage()
                {
                    Count = item.Count,
                    ProductId = item.ProductId
                });
            });

            await _publishEndPoint.Publish(orderCreatedEvent);

            return Ok();
        }
    }
}
