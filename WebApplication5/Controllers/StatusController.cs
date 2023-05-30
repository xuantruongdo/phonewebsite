using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTOPost;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        public readonly DataContext _dbContext;
        public StatusController(DataContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpGet]
        public async Task<List<Status>> GetStatuses()
        {
            return await _dbContext.Statuses.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> PostStatus(StatusDTO req)
        {
            var newStatus = new Status
            {
                Name = req.Name
            };
            _dbContext.Statuses.Add(newStatus);
            await _dbContext.SaveChangesAsync();
            return Ok(newStatus);
        }
    }
}
