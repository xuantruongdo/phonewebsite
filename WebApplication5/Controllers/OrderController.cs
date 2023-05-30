using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;
using WebApplication5.DTO_Put;
using WebApplication5.DTOPut;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public readonly DataContext _dbContext;
        public OrderController(DataContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        [HttpGet]
        public async Task<List<Order>> GetOrders()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> PostOrder(OrderDTO req)
        {
            var newOrder = new Order
            {
                TotalPrice = req.TotalPrice,
                UserId = req.UserId,
                PaymentId = req.PaymentId,
                StatusId = req.StatusId,
                Fullname = req.Fullname,
                Phone = req.Phone,
                Address= req.Address,

            };
            _dbContext.Orders.Add(newOrder);
            await _dbContext.SaveChangesAsync();
            return Ok(newOrder);
        }
        [HttpPut("status/{id}")]
        public async Task<ActionResult<Order>> PutStatusOrder(int id, OrderStatusDTO req)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.StatusId = req.StatusId;

            try
            {
                await _dbContext.SaveChangesAsync();
                return order;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool OrderExists(int id)
        {
            return _dbContext.Orders.Any(e => e.Id == id);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            if (_dbContext.Orders == null)
            {
                return NotFound();
            }

            var order = await _dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            _dbContext.Orders.Remove(order);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("history/User/{userId}")]
        public async Task<ActionResult<List<Order>>> GetHistoryOrder(int userId)
        {
            var orders = await _dbContext.Orders
                .Where(p => p.UserId == userId)
                .ToListAsync();

            if (orders == null)
            {
                return NotFound();
            }

            return orders;
        }
    }
}
