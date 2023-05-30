using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;
using WebApplication5.DTOPost;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public readonly DataContext _dbContext;
        public PaymentController(DataContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpGet]
        public async Task<List<Payment>> GetPayments()
        {
            return await _dbContext.Payments.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> PostPayment(PaymentDTO req)
        {
            var newPayment = new Payment
            {
                Name = req.Name
            };
            _dbContext.Payments.Add(newPayment);
            await _dbContext.SaveChangesAsync();
            return Ok(newPayment);
        }
    }
}
