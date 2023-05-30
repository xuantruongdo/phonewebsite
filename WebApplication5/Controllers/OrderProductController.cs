using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : ControllerBase
    {
        public readonly DataContext _dbContext;
        public OrderProductController(DataContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderProductDTO>> GetOrderProduct(int id)
        {
            var orderProducts = _dbContext.OrderProducts.Where(op => op.ProductId == id).ToList();
            if (orderProducts.Count() == 0)
            {
                return NotFound();
            }
            return Ok(orderProducts);
        }
        [HttpPost]
        public async Task<ActionResult> PostOrderProduct(OrderProductDTO req)
        {
            var newOrderProduct = new OrderProduct
            {
                OrderId = req.OrderId,
                ProductId = req.ProductId,
                ColorId = req.ColorId,
                Quantity = req.Quantity,
            };
            _dbContext.OrderProducts.Add(newOrderProduct);
            await _dbContext.SaveChangesAsync();
            return Ok(newOrderProduct);
        }

        [HttpGet("history/product/{productId}")]
        public async Task<ActionResult<List<OrderProduct>>> GetProdut(int productId)
        {
            var orderProducts = await _dbContext.OrderProducts
                .Where(p => p.ProductId == productId)
                .ToListAsync();

            if (orderProducts == null)
            {
                return NotFound();
            }

            return orderProducts;
        }

        [HttpGet("history/order/{orderId}")]
        public async Task<ActionResult<List<OrderProduct>>> GetOrder(int orderId)
        {
            var orderProducts = await _dbContext.OrderProducts
                .Where(p => p.OrderId == orderId)
                .ToListAsync();

            if (orderProducts == null)
            {
                return NotFound();
            }

            return orderProducts;
        }
    }
}
