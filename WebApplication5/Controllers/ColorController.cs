using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        public readonly DataContext _dbContext;
        public ColorController(DataContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        [HttpGet]
        public async Task<List<Color>> GetColors()
        {
            return await _dbContext.Colors.ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult> PostBrand(ColorDTO req)
        {
            var newColor = new Color
            {
                ColorName = req.ColorName,
                Stock = req.Stock,
                ProductId = req.ProductId,
            };
            _dbContext.Colors.Add(newColor);
            await _dbContext.SaveChangesAsync();
            return Ok(newColor);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> GetColor(int id)
        {
            var color = await _dbContext.Colors.FirstOrDefaultAsync(b => b.Id == id);

            if (color == null)
            {
                return NotFound();
            }

            return color;
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<List<Color>>> GetColorProduct(int productId)
        {
            var colors = await _dbContext.Colors
                .Where(p => p.ProductId == productId)
                .ToListAsync();

            if (colors == null)
            {
                return NotFound();
            }

            return colors;
        }
    }
}
