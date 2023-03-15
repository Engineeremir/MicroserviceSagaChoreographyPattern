using MassTransit;
using MicroserviceSagaPattern.Order.API.Dtos;
using MicroserviceSagaPattern.Order.API.Models;
using MicroserviceSagaPattern.Shared;
using MicroserviceSagaPattern.Shared.Events.Order;
using MicroserviceSagaPattern.Shared.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceSagaPattern.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPublishEndpoint _publishEndPoint;

        public OrdersController(ApplicationDbContext context,IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndPoint = publishEndpoint;
        }

        
        
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto orderCreateDto)
        {
            var newOrder = new Models.Order
            {
                BuyerId= orderCreateDto.BuyerId,
                OrderStatus = OrderStatus.Suspended,
                Address = new Address
                {
                    Line = orderCreateDto.Address.Line,
                    District= orderCreateDto.Address.District,
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
                    CardName= orderCreateDto.Payment.CardName,
                    CardNumber= orderCreateDto.Payment.CardNumber,
                    Expiration = orderCreateDto.Payment.Expiration,
                    CVV = orderCreateDto.Payment.CVV,
                    TotalPrice = orderCreateDto.OrderItems.Sum(x=>x.Price*x.Count),
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
